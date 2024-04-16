using Microsoft.EntityFrameworkCore;

using PortfolioManager.Domain.Common;
using PortfolioManager.Domain.InvestmentAggregate;
using PortfolioManager.Infrastructure.Persistence.Commom;

namespace PortfolioManager.Infrastructure.Persistence.InvestmentAggregate;

internal class InvestmentRepository(AppDbContext context) : IInvestmentRepository
{
    public PaginatedList<Investment> GetPaginated(int pageIndex, int pageSize, bool available = true)
    {
        var query = context.Investments.AsQueryable();

        if (available)
            query = query.Where(x => x.IsAvailable == true);

        return PaginatedList<Investment>.Create(query, pageIndex, pageSize);
    }

    public async Task<Investment?> GetAvailableById(Guid id, CancellationToken cancellationToken)
    {
        return await context.Investments
            .Where(x => x.IsAvailable == true)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<Investment?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await context.Investments
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }
}