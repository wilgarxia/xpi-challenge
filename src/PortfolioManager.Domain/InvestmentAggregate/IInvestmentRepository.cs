using PortfolioManager.Domain.InvestmentAggregate;

namespace PortfolioManager.Domain.Common;

public interface IInvestmentRepository
{
    IQueryable<Investment> GetQueryForPagination(bool available = true);
    Task<Investment?> GetAvailableById(Guid id, CancellationToken cancellationToken);
    Task<Investment?> GetById(Guid id, CancellationToken cancellationToken);
}