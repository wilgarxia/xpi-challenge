using Microsoft.EntityFrameworkCore;

using PortfolioManager.Domain.Products;
using PortfolioManager.Infrastructure.Common;
using PortfolioManager.Infrastructure.Persistence.Commom;

namespace PortfolioManager.Infrastructure.Persistence.Products;

internal class ProductRepository(AppDbContext context, IDateTimeProvider dateTimeProvider) : IProductRepository
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public IQueryable<Product> GetQueryForPagination() =>
        context.Products
            .Where(x => x.IsAvailable == true)
            .AsQueryable();

    public async Task<Product?> GetById(Guid id, CancellationToken cancellationToken) =>
            await context.Products
                .Where(x => x.IsAvailable == true)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<Product?> GetByDescription(string description, CancellationToken cancellationToken) =>
            await context.Products
                .Where(x => x.IsAvailable == true)
                .FirstOrDefaultAsync(x => x.Description == description, cancellationToken);

    public async Task Add(Product product, CancellationToken cancellationToken)
    {
        await context.Products.AddAsync(product, cancellationToken);
        _ = await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductsCloseToDueDate(CancellationToken ct)
    {
        return await context.Products
            .Where(p => p.DueAt <= _dateTimeProvider.UtcNow.AddDays(30) && p.DueAt >= _dateTimeProvider.UtcNow)
            .ToListAsync(ct);
    }
}