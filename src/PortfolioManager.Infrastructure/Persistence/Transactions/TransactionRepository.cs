using Microsoft.EntityFrameworkCore;

using PortfolioManager.Domain.Users;
using PortfolioManager.Infrastructure.Persistence.Commom;

namespace PortfolioManager.Infrastructure.Persistence.Transactions;

internal class TransactionRepository(AppDbContext context) : ITransactionRepository
{
    public IQueryable<Transaction> GetUserQueryForPagination(Guid userId) =>
        context.Transactions
            .Include(x => x.Product)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .AsQueryable();

    public async Task<Transaction?> GetUserTransactionsById(Guid userId, Guid id, CancellationToken cancellationToken) =>
            await context.Transactions.FirstOrDefaultAsync(u => u.UserId == userId && u.Id == id, cancellationToken);

    public async Task Add(Transaction transaction, CancellationToken cancellationToken) =>
        await context.Transactions.AddAsync(transaction, cancellationToken);
}