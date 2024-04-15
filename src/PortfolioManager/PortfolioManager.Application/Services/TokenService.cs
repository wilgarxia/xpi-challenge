using FluentResults;

using Microsoft.EntityFrameworkCore;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Infrastructure.Persistence;
using PortfolioManager.Infrastructure.Security;

namespace PortfolioManager.Application.Services;

public interface ITokenService
{
    Task<Result<string>> CreateToken(CreateTokenRequest request, CancellationToken cancellationToken);
}

public class TokenService(IJwtProvider jwtProvider, ApplicationDbContext context) : ITokenService
{
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<string>> CreateToken(CreateTokenRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result.Fail("Invalid username or password.");

        string token = _jwtProvider.Generate(user.Id.ToString(), user.Username, user.IsAdmin);

        return Result.Ok(token);
    }
}