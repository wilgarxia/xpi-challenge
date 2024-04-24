using System.Text.Json;

using ErrorOr;

using FluentValidation;

using Microsoft.Extensions.Caching.Distributed;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Interfaces;
using PortfolioManager.Domain.Products;
using PortfolioManager.Domain.Users;
using PortfolioManager.Infrastructure.Common;
using PortfolioManager.Infrastructure.Persistence.Commom;
using PortfolioManager.Infrastructure.Security.CurrentUser;

namespace PortfolioManager.Application.Services;

internal class ProductsService(
    AppDbContext context,
    IProductRepository repository,
    IDateTimeProvider dateTimeProvider,
    ICurrentUserProvider currentUserProvider,
    ITransactionRepository transactionRepository,
    IValidator<CreateProductRequest> createValidator,
    IValidator<UpdateProductRequest> updateValidator,
    IDistributedCache cache) : IProductsService
{
    private readonly AppDbContext _context = context;
    private readonly IProductRepository _productRepository = repository;
    private readonly IDateTimeProvider  _dateTimeProvider = dateTimeProvider;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IValidator<CreateProductRequest> _createValidator = createValidator;
    private readonly IValidator<UpdateProductRequest> _updateValidator = updateValidator;
    private readonly IDistributedCache _cache = cache;

    public async Task<ErrorOr<PaginatedList<ProductResponse>>> GetAll(GetAllProductsRequest request, CancellationToken ct)
    {
        var cacheKey = $"products_{request.PageIndex}_{request.PageSize}";
        var cachedData = await _cache.GetStringAsync(cacheKey, ct);

        if (!string.IsNullOrEmpty(cachedData))
        {
            var cachedResults = JsonSerializer.Deserialize<List<ProductResponse>>(cachedData);

            return PaginatedList<ProductResponse>.Create(cachedResults!, request.PageIndex, request.PageSize);
        }

        var results = PaginatedList<Product>.CreateMapped(
            _productRepository.GetQueryForPagination(), 
            request.PageIndex, 
            request.PageSize, 
            i => ToResponse(i) 
        );

        var serializedResults = JsonSerializer.Serialize(results.Items);

        await _cache.SetStringAsync(cacheKey, serializedResults, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        }, ct);

        return results;
    }

    public async Task<ErrorOr<ProductResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var cacheKey = $"product_{id}";
        var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (!string.IsNullOrEmpty(cachedData))
        {
            var cachedProduct = JsonSerializer.Deserialize<ProductResponse>(cachedData);

            if (cachedProduct != null)
            {
                return cachedProduct;
            }
        }

        var product = await _productRepository.GetById(id, cancellationToken);

        if (product is null)
            return Error.NotFound(description: "Product not found.");

        var productResponse = ToResponse(product);
        var serializedProduct = JsonSerializer.Serialize(productResponse);
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) 
        };
        await _cache.SetStringAsync(cacheKey, serializedProduct, cacheOptions, cancellationToken);

        return ToResponse(product);
    }

    public async Task<ErrorOr<ProductResponse>> CreateProduct(CreateProductRequest request, CancellationToken cancellationToken)
    {
        // TODO: Cache invalidation
        if (await _createValidator.ValidateAsync(request, cancellationToken) is var validation && !validation.IsValid)
            return validation.Errors.ConvertAll(error => Error.Validation(error.PropertyName, error.ErrorMessage));

        if (await _productRepository.GetByDescription(request.Description, cancellationToken)
                is var result && result is not null)
            return ProductFailures.ProductAlreadyExists;

        var product = Product.Create(
            _dateTimeProvider.UtcNow, 
            request.Description, 
            request.DueAt.Date, 
            request.MinimumInvestmentAmount, 
            request.ManagerEmail);

        await _productRepository.Add(product, cancellationToken);

        return ToResponse(product);
    }

    public async Task<ErrorOr<ProductResponse>> UpdateProduct(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        // TODO: Cache invalidation
        if (await _updateValidator.ValidateAsync(request, cancellationToken) is var validation && !validation.IsValid)
            return validation.Errors.ConvertAll(error => Error.Validation(error.PropertyName, error.ErrorMessage));

        if (await _productRepository.GetById(request.Id, cancellationToken) is var product && product is null)
            return Error.NotFound(description: "Product not found.");

        product.Update(_dateTimeProvider.UtcNow, request.Description, request.MinimumInvestmentAmount);

        await _context.SaveChangesAsync(cancellationToken);

        return ToResponse(product);
    }

    public async Task<ErrorOr<ProductResponse>> DeactivateProduct(Guid id, CancellationToken cancellationToken)
    {
        // TODO: Cache invalidation
        if (await _productRepository.GetById(id, cancellationToken) is var product && product is null)
            return Error.NotFound(description: "Product not found.");

        if (!product.IsAvailable)
            return ProductFailures.ProductUnavailable;

        product.Deactivate(_dateTimeProvider.UtcNow);

        await _context.SaveChangesAsync(cancellationToken);

        return ToResponse(product);
    }

    public async Task<ErrorOr<TransactionResponse>> BuyProduct(BuyOrSellProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetById(request.ProductId, cancellationToken);

        if (product is null)
            return Error.NotFound(description: "Product not found.");

        if (request.Amount < product.MinimumInvestmentAmount)
            return ProductFailures.AmountLessThanMinimumRequired;

        var user = await _currentUserProvider.GetCurrentUserWithPortfolio(cancellationToken);

        if (user is null)
            return Error.NotFound(description: "User not found.");

        using var dbTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        if (user.PortfolioProducts.Any(x => x.ProductId == request.ProductId))
        {
            var userProduct = user.PortfolioProducts
                .Where(x => x.ProductId == request.ProductId)
                .Single();

            userProduct.Increase(_dateTimeProvider.UtcNow, request.Amount);
        }
        else
            user.AddProductToPortfolio(_dateTimeProvider.UtcNow, product.Id, user.Id, request.Amount);

        var transaction = Transaction.CreateDebitTransaction(
            _dateTimeProvider.UtcNow,
            $"Buy - {product.Description[..6]}",
            request.Amount,
            user.Id,
            product.Id);

        await _transactionRepository.Add(transaction, cancellationToken);
        _ = await _context.SaveChangesAsync(cancellationToken);

        await dbTransaction.CommitAsync(cancellationToken);

        return ToResponse(transaction);
    }

    public async Task<ErrorOr<TransactionResponse>> SellProduct(BuyOrSellProductRequest request, CancellationToken cancellationToken)
    {
        var user = await _currentUserProvider.GetCurrentUserWithPortfolio(cancellationToken);

        if (user is null)
            return Error.NotFound(description: "User not found.");

        if (!user.PortfolioProducts.Any(x => x.ProductId == request.ProductId))
            return Error.NotFound(description: "Product not found.");

        var userProduct = user.PortfolioProducts
            .Where(x => x.ProductId == request.ProductId)
            .Single();

        if (userProduct.Amount < request.Amount)
            return ProductFailures.AmountLessThanMinimumRequired;

        if (await _productRepository.GetById(request.ProductId, cancellationToken) is var product && product is null)
            return Error.NotFound(description: "Product not found.");

        userProduct.Descrease(_dateTimeProvider.UtcNow, request.Amount);

        var transaction = Transaction.CreateCreditTransaction(
            _dateTimeProvider.UtcNow,
            $"Buy - {product.Description[..6]}",
            request.Amount,
            user.Id,
            product.Id);

        await _transactionRepository.Add(transaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ToResponse(transaction);
    }

    private static TransactionResponse ToResponse(Transaction transaction) =>
        new()
        {
            Id = transaction.Id,
            CreatedAt = transaction.CreatedAt,
            Amount = transaction.Amount,
            Operation = transaction.Direction switch
            {
                TransactionDirection.Debit => TransactionType.Buy,
                TransactionDirection.Credit => TransactionType.Sell,
                _ => throw new InvalidOperationException()
            },
            Product = new TransactionResponse.TransactionProduct()
            {
                Id = transaction.Product.Id,
                Description = transaction.Product.Description
            }
        };

    private static ProductResponse ToResponse(Product product) =>
        new(product.Id,
            product.CreatedAt,
            product.UpdatedAt,
            product.Description,
            product.DueAt,
            product.MinimumInvestmentAmount);
}