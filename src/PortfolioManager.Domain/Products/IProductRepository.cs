namespace PortfolioManager.Domain.Products;

public interface IProductRepository
{
    IQueryable<Product> GetQueryForPagination();
    Task<Product?> GetById(Guid id, CancellationToken cancellationToken);
    Task<Product?> GetByDescription(string description, CancellationToken cancellationToken);
    Task Add(Product product, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetProductsCloseToDueDate(CancellationToken ct);
}