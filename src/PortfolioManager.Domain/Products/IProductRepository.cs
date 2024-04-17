namespace PortfolioManager.Domain.Products;

public interface IProductRepository
{
    IQueryable<Product> GetQueryForPagination();
    Task<Product?> GetById(Guid id, CancellationToken cancellationToken);
    Task Add(Product product, CancellationToken cancellationToken);
}