using ErrorOr;

using PortfolioManager.Application.Contracts;

namespace PortfolioManager.Application.Interfaces;

public interface IPortfolioService
{
    Task<ErrorOr<PaginatedList<PortfolioProductResponse>>> GetPortfolio(
        GetPortfolioRequest request, CancellationToken ct);
}