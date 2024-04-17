using Microsoft.EntityFrameworkCore;

using PortfolioManager.Domain.Products;
using PortfolioManager.Infrastructure.Persistence.Commom;

namespace PortfolioManager.Infrastructure.Persistence.Products;

internal class ProductRepository(AppDbContext context) : IProductRepository
{
    public IQueryable<Product> GetQueryForPagination() =>
        context.Products
            .Where(x => x.IsAvailable == true)
            .AsQueryable();

    public async Task<Product?> GetById(Guid id, CancellationToken cancellationToken) =>
            await context.Products
                .Where(x => x.IsAvailable == true)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task Add(Product product, CancellationToken cancellationToken) =>
        await context.Products.AddAsync(product, cancellationToken);
}