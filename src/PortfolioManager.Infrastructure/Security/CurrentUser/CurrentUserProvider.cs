using Microsoft.AspNetCore.Http;

using PortfolioManager.Domain.UserAggregate;

namespace PortfolioManager.Infrastructure.Security.CurrentUser;

public interface ICurrentUserProvider
{
    Task<User?> GetCurrentUser(CancellationToken cancellationToken);
}

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository) : ICurrentUserProvider
{
    public async Task<User?> GetCurrentUser(CancellationToken cancellationToken)
    {
        var id = httpContextAccessor
            .HttpContext!
            .User
            .Claims
            .Single(claim => claim.Type == "userid")
            .Value;

        if (!Guid.TryParse(id, out Guid userId))
            return null;

        var user = await userRepository.GetById(userId, cancellationToken);

        return user;
    }
}