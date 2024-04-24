using Microsoft.EntityFrameworkCore;

using PortfolioManager.Domain.Users;
using PortfolioManager.Infrastructure.Persistence.Commom;

namespace PortfolioManager.Infrastructure.Persistence.Users;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetById(Guid id, CancellationToken cancellationToken) =>
        await context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<User?> GetByIdWithPotfolio(Guid id, CancellationToken cancellationToken) =>
        await context.Users.Include(x => x.PortfolioProducts).ThenInclude(x => x.Product).FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<User?> GetByEmail(string email, CancellationToken cancellationToken) =>
        await context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task Add(User user, CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(user, cancellationToken);
        _ = await context.SaveChangesAsync(cancellationToken);
    }
        
    public async Task UpdatePortfolio(CancellationToken cancellationToken)
    {
        var addedEntries = context.PortfolioProducts
            .Where(x => context.PortfolioProducts.Entry(x).State == EntityState.Added);

        await context.PortfolioProducts.AddRangeAsync(addedEntries, cancellationToken);

        var removedEntries = context.PortfolioProducts
            .Where(x => context.PortfolioProducts.Entry(x).State == EntityState.Deleted)
            .ToList();

        context.PortfolioProducts.RemoveRange(removedEntries);

        var changedEntries = context.PortfolioProducts.Where(x => context.PortfolioProducts.Entry(x).State == EntityState.Modified);

        context.PortfolioProducts.UpdateRange(changedEntries);
    }
}