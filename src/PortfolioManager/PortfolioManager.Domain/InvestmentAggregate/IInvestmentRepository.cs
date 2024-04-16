using PortfolioManager.Domain.InvestmentAggregate;
using PortfolioManager.Infrastructure.Persistence.Commom;

namespace PortfolioManager.Domain.Common;

public interface IInvestmentRepository
{
    PaginatedList<Investment> GetPaginated(int pageIndex, int pageSize, bool available = true);
    Task<Investment?> GetAvailableById(Guid id, CancellationToken cancellationToken);
    Task<Investment?> GetById(Guid id, CancellationToken cancellationToken);
}