using Microsoft.EntityFrameworkCore;

using PortfolioManager.Domain.Users;
using PortfolioManager.Infrastructure.Persistence.Commom;

namespace PortfolioManager.Infrastructure.Persistence.Users;

public class PortfolioProductRepository(AppDbContext context) : IPortfolioProductRepository
{
    public IQueryable<PortfolioProduct> GetPortfolioQueryForPagination(Guid userId) =>
        context.PortfolioProducts
            .Include(x => x.Product)
            .Where(x => x.Amount > 0)
            .AsQueryable();
}