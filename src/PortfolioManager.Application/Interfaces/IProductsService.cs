using System.Threading.Tasks;

using ErrorOr;

using PortfolioManager.Application.Contracts;

namespace PortfolioManager.Application.Interfaces;

public interface IProductsService
{
    Task<ErrorOr<PaginatedList<ProductResponse>>> GetAll(GetAllProductsRequest request, CancellationToken ct);
    Task<ErrorOr<ProductResponse>> GetById(Guid id, CancellationToken cancellationToken);
    Task<ErrorOr<ProductResponse>> CreateProduct(CreateProductRequest request, CancellationToken cancellationToken);
    Task<ErrorOr<ProductResponse>> UpdateProduct(UpdateProductRequest request, CancellationToken cancellationToken);
    Task<ErrorOr<ProductResponse>> DeactivateProduct(Guid id, CancellationToken cancellationToken);
    Task<ErrorOr<TransactionResponse>> BuyProduct(BuyOrSellProductRequest request, CancellationToken cancellationToken);
    Task<ErrorOr<TransactionResponse>> SellProduct(BuyOrSellProductRequest request, CancellationToken cancellationToken);
}