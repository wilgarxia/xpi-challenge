namespace PortfolioManager.Domain.Users;

public interface ITransactionRepository
{
    IQueryable<Transaction> GetUserQueryForPagination(Guid userId);
    Task<Transaction?> GetUserTransactionsById(Guid userId, Guid id, CancellationToken cancellationToken);
    Task Add(Transaction transaction, CancellationToken cancellationToken);
}