using FluentResults;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Domain.Common;
using PortfolioManager.Domain.InvestmentAggregate;
using PortfolioManager.Domain.UserAggregate;
using PortfolioManager.Infrastructure.Security.CurrentUser;

namespace PortfolioManager.Application.Services;

public interface IInvestmentService
{
    Task<Result<PaginatedList<Investment>>> GetAll(GetAllInvestmentsRequest request, CancellationToken cancellationToken);
    Task<Result<Investment?>> GetById(Guid id, CancellationToken cancellationToken);
}

internal class InvestmentService(IInvestmentRepository repository, ICurrentUserProvider currentUserProvider) : IInvestmentService
{
    public async Task<Result<PaginatedList<Investment>>> GetAll(GetAllInvestmentsRequest request, CancellationToken cancellationToken)
    {
        User? currentUser = await currentUserProvider.GetCurrentUser(cancellationToken);

        if (currentUser is null)
            return Result.Fail("");

        bool returnOnlyAvailable = true;

        if (currentUser.IsAdmin)
            returnOnlyAvailable = false;

        var query = repository.GetQueryForPagination(returnOnlyAvailable);
        var results = PaginatedList<Investment>.Create(query, request.PageIndex, request.PageSize);

        return Result.Ok(results);
    }

    public async Task<Result<Investment?>> GetById(Guid id, CancellationToken cancellationToken)
    {
        User? currentUser = await currentUserProvider.GetCurrentUser(cancellationToken);

        if (currentUser is null)
            return Result.Fail("");

        Investment? investment = currentUser.IsAdmin ? 
            await repository.GetById(id, cancellationToken) :
            await repository.GetAvailableById(id, cancellationToken);

        return Result.Ok(investment);
    }
}