using Microsoft.AspNetCore.Http;

using PortfolioManager.Domain.Users;

namespace PortfolioManager.Infrastructure.Security.CurrentUser;

public interface ICurrentUserProvider
{
    Task<User?> GetCurrentUser(CancellationToken cancellationToken);
    Task<User?> GetCurrentUserWithPortfolio(CancellationToken cancellationToken);
}

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _contextAccessor; 
    private readonly IUserRepository _userRepository;

    public CurrentUserProvider(IHttpContextAccessor contextAccessor, IUserRepository userRepository)
    {
        ArgumentNullException.ThrowIfNull(contextAccessor);
        ArgumentNullException.ThrowIfNull(userRepository);

        _contextAccessor = contextAccessor;
        _userRepository = userRepository;
    }

    public async Task<User?> GetCurrentUser(CancellationToken cancellationToken)
    {
        var id = _contextAccessor
            .HttpContext!
            .User
            .Claims
            .Single(claim => claim.Type == "userid")
            .Value;

        if (!Guid.TryParse(id, out Guid userId))
            return null;

        var user = await _userRepository.GetById(userId, cancellationToken);

        return user;
    }

    public async Task<User?> GetCurrentUserWithPortfolio(CancellationToken cancellationToken)
    {
        var id = _contextAccessor
            .HttpContext!
            .User
            .Claims
            .Single(claim => claim.Type == "userid")
            .Value;

        if (!Guid.TryParse(id, out Guid userId))
            return null;

        var user = await _userRepository.GetByIdWithPotfolio(userId, cancellationToken);

        return user;
    }
}