using ErrorOr;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Interfaces;
using PortfolioManager.Domain.Users;
using PortfolioManager.Infrastructure.Security.CurrentUser;

namespace PortfolioManager.Application.Services;

public class TransactionService(
    ITransactionRepository repository,
    ICurrentUserProvider currentUserProvider) : ITransactionService
{
    private readonly ITransactionRepository _repository = repository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public async Task<ErrorOr<PaginatedList<TransactionResponse>>> GetAll(
        GetAllTransactionsRequest request, CancellationToken cancellationToken)
    {
        var user = await _currentUserProvider.GetCurrentUser(cancellationToken);

        if (user is null)
            return Error.NotFound(description: "User not found.");

        var results = PaginatedList<Transaction>.CreateMapped(
            _repository.GetUserQueryForPagination(user.Id),
            request.PageIndex,
            request.PageSize,
            i => ToResponse(i)
        );

        return results;
    }

    private static TransactionResponse ToResponse(Transaction transaction) => 
        new()
        {
            Id = transaction.Id,
            CreatedAt = transaction.CreatedAt,
            Amount = transaction.Amount,
            Operation = transaction.Direction switch
            {
                TransactionDirection.Debit => TransactionType.Buy,
                TransactionDirection.Credit => TransactionType.Sell,
                _ => throw new NotImplementedException()
            },
            Product = new TransactionResponse.TransactionProduct()
            {
                Id = transaction.Product.Id,
                Description = transaction.Product.Description
            }
        };
}
