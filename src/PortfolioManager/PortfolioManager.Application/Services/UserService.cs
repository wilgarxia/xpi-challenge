using FluentResults;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Domain.UserAggregate;

namespace PortfolioManager.Application.Services;

public interface IUserService
{
    Task<Result<CreateUserResponse>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken);
}

public class UserService(IUserRepository repository) : IUserService
{
    public async Task<Result<CreateUserResponse>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
    {
        User? user = await repository.GetByUsername(request.Username, cancellationToken);

        if (user is not null)
            return Result.Fail("User already exists");

        user = new()
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            IsAdmin = request.IsAdmin
        };

        await repository.Add(user, cancellationToken);
        await repository.SaveChanges(cancellationToken);

        CreateUserResponse result = new(user.Id, user.Username);

        return Result.Ok(result);
    }
}