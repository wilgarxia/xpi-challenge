namespace PortfolioManager.Domain.UserAggregate;

public interface IUserRepository
{
    Task<User?> GetByUsername(string username, CancellationToken cancellationToken);
    Task Add(User user, CancellationToken cancellationToken);
    Task SaveChanges(CancellationToken cancellationToken);
}
