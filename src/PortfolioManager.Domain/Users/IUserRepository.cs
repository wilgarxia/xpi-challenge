namespace PortfolioManager.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetById(Guid id, CancellationToken cancellationToken);
    Task<User?> GetByEmail(string email, CancellationToken cancellationToken);
    Task Add(User user, CancellationToken cancellationToken);
}