using FluentResults;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Domain.UserAggregate;
using PortfolioManager.Infrastructure.Security;

namespace PortfolioManager.Application.Services;

public interface ITokenService
{
    Task<Result<string>> CreateToken(CreateTokenRequest request, CancellationToken cancellationToken);
}

public class TokenService(IJwtProvider jwtProvider, IUserRepository repository) : ITokenService
{
    public async Task<Result<string>> CreateToken(CreateTokenRequest request, CancellationToken cancellationToken)
    {
        User? user = await repository.GetByUsername(request.Username, cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result.Fail("Invalid username or password.");

        string token = jwtProvider.Generate(user.Id.ToString(), user.Username, user.IsAdmin);

        return Result.Ok(token);
    }
}