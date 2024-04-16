using Microsoft.EntityFrameworkCore;

using PortfolioManager.Domain.UserAggregate;
using PortfolioManager.Infrastructure.Persistence.Commom;

namespace PortfolioManager.Infrastructure.Persistence.UserAggregate;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetById(Guid id, CancellationToken cancellationToken) =>
        await context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<User?> GetByUsername(string username, CancellationToken cancellationToken) =>
        await context.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

    public async Task Add(User user, CancellationToken cancellationToken) =>
        await context.Users.AddAsync(user, cancellationToken);

    public async Task SaveChanges(CancellationToken cancellationToken) =>
        _ = await context.SaveChangesAsync(cancellationToken);
}