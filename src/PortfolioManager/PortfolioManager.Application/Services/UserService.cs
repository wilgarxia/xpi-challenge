using FluentResults;

using Microsoft.EntityFrameworkCore;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Infrastructure.Persistence;

namespace PortfolioManager.Application.Services;

public interface IUserService
{
    Task<Result<CreateUserResponse>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken);
}

public class UserService(ApplicationDbContext context) : IUserService
{
    public async Task<Result<CreateUserResponse>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await context.User.FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

        if (user is not null)
            return Result.Fail("User already exists");

        user = new()
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            IsAdmin = request.IsAdmin
        };

        await context.User.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        CreateUserResponse result = new(user.Id, user.Username);

        return Result.Ok(result);
    }
}