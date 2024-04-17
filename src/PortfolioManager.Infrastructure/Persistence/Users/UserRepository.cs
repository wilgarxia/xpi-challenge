using Microsoft.EntityFrameworkCore;

using PortfolioManager.Domain.Users;
using PortfolioManager.Infrastructure.Persistence.Commom;

namespace PortfolioManager.Infrastructure.Persistence.Users;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetById(Guid id, CancellationToken cancellationToken) =>
        await context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<User?> GetByEmail(string email, CancellationToken cancellationToken) =>
        await context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task Add(User user, CancellationToken cancellationToken) =>
        await context.Users.AddAsync(user, cancellationToken);
}