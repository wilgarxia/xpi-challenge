using ErrorOr;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Interfaces;
using PortfolioManager.Domain.Users;
using PortfolioManager.Infrastructure.Security.CurrentUser;

namespace PortfolioManager.Application.Services;

internal class PortfolioService(
    IPortfolioProductRepository repository,
    ICurrentUserProvider currentUserProvider) : IPortfolioService
{
    private readonly IPortfolioProductRepository _repository = repository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public async Task<ErrorOr<PaginatedList<PortfolioProductResponse>>> GetPortfolio(
        GetPortfolioRequest request, CancellationToken ct)
    {
        var user = await _currentUserProvider.GetCurrentUserWithPortfolio(ct);

        if (user is null)
            return Error.NotFound(description: "User not found.");

        var results = PaginatedList<PortfolioProduct>.CreateMapped(
            _repository.GetPortfolioQueryForPagination(user.Id),
            request.PageIndex,
            request.PageSize,
            i => ToResponse(i)
        );

        return results;
    }

    private static PortfolioProductResponse ToResponse(PortfolioProduct portfolio) =>
        new(
            new(
                portfolio.Product.Id,
                portfolio.Product.CreatedAt,
                portfolio.Product.UpdatedAt,
                portfolio.Product.Description,
                portfolio.Product.DueAt,
                portfolio.Product.MinimumInvestmentAmount),
            portfolio.Amount);
}