using ErrorOr;

using PortfolioManager.Application.Contracts;

namespace PortfolioManager.Application.Interfaces;

public interface ITransactionService
{
    Task<ErrorOr<PaginatedList<TransactionResponse>>> GetAll(GetAllTransactionsRequest request, CancellationToken cancellationToken);
}