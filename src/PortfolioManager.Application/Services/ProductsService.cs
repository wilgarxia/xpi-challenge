using FluentResults;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Domain.Products;
using PortfolioManager.Infrastructure.Persistence.Commom;

namespace PortfolioManager.Application.Services;

public interface IProductsService
{
    Result<PaginatedList<ProductResponse?>> GetAll(GetAllProductsRequest request);
    Task<Result<Product?>> GetById(Guid id, CancellationToken cancellationToken);
    Task<Result<ProductResponse?>> AddProduct(AddProductRequest request, CancellationToken cancellationToken);
    Task<Result<ProductResponse?>> UpdateProduct(UpdateProductRequest request, CancellationToken cancellationToken);
}

internal class ProductsService : IProductsService
{
    private readonly AppDbContext _context;
    private readonly IManagerRepository _managerRepository;
    private readonly IProductRepository _productRepository;

    public ProductsService(
        AppDbContext context,
        IManagerRepository managerRepository, 
        IProductRepository repository/*, ICurrentUserProvider currentUserProvider*/)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(managerRepository);
        //ArgumentNullException.ThrowIfNull(currentUserProvider);
        _context = context;
        _productRepository = repository;
        _managerRepository = managerRepository;
        //_currentUserProvider = currentUserProvider;
    }

    public Result<PaginatedList<ProductResponse?>> GetAll(GetAllProductsRequest request)
    {
        var results = PaginatedList<Product>.CreateMapped(
            _productRepository.GetQueryForPagination(), 
            request.PageIndex, 
            request.PageSize, 
            i => ToResponse(i) 
        );

        return Result.Ok(results);
    }

    public async Task<Result<Product?>> GetById(Guid id, CancellationToken cancellationToken) =>
        Result.Ok(await _productRepository.GetById(id, cancellationToken));

    public async Task<Result<ProductResponse?>> AddProduct(AddProductRequest request, CancellationToken cancellationToken)
    {
        var manager = await _managerRepository.GetByFirstNameAndLastName(
            request.Manager.FirstName, request.Manager.LastName, cancellationToken);

        if (manager is null)
        {
            manager = Manager.Create(request.Manager.FirstName, request.Manager.LastName, request.Manager.Email);

            await _managerRepository.Add(manager, cancellationToken);
        }

        var product = new Product
        {
            Description = request.Description,
            DueAt = request.DueAt,
            MinimumInvestmentAmount = request.MinimumInvestmentAmount,
            IsAvailable = true,
            Manager = manager
        };

        await _productRepository.Add(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok(ToResponse(product));
    }

    public async Task<Result<ProductResponse?>> UpdateProduct(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        if (await _productRepository.GetById(request.Id, cancellationToken) is var product && product is null)
            return Result.Ok();

        product.Description = request.Description;
        product.MinimumInvestmentAmount = request.MinimumInvestmentAmount;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok(ToResponse(product));
    }

    private static ProductResponse? ToResponse(Product product) =>
         new(product.Id,
             product.CreatedAt,
             product.UpdatedAt,
             product.Description,
             product.DueAt,
             product.MinimumInvestmentAmount);

    //public async Task<Result<bool>> SoftDeleteInvestment(Guid id, CancellationToken cancellationToken)
    //{
    //    var currentUser = await _currentUserProvider.GetCurrentUser(cancellationToken);
    //    if (currentUser is null)
    //        return Result.Fail<bool>("User not found.");

    //    var investment = await _repository.GetById(id, cancellationToken);
    //    if (investment == null)
    //        return Result.Fail<bool>("Investment not found.");

    //    if (!currentUser.IsAdmin && investment.UserId != currentUser.Id)
    //        return Result.Fail<bool>("Unauthorized access.");

    //    investment.IsAvailable = false; // Soft delete by marking as not available
    //    investment.UpdatedAt = DateTime.UtcNow; // Update the modified time

    //    await _repository.Update(investment, cancellationToken);
    //    await _repository.SaveChanges(cancellationToken);

    //    return Result.Ok(true);
    //}
}