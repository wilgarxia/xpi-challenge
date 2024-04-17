using FluentResults;

using FluentValidation;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Extensions;
using PortfolioManager.Domain.Users;
using PortfolioManager.Infrastructure.Security.Jwt;

namespace PortfolioManager.Application.Services;

public interface IAuthenticationService
{
    Task<Result<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenProvider _jwtProvider;
    private readonly IUserRepository _repository;
    private readonly IValidator<LoginRequest> _validator;

    public AuthenticationService(
        IJwtTokenProvider jwtProvider, IUserRepository repository, IValidator<LoginRequest> validator)
    {
        ArgumentNullException.ThrowIfNull(jwtProvider);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(validator);

        _jwtProvider = jwtProvider;
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        if (await _validator.ValidateAsync(request, cancellationToken) is var validation && !validation.IsValid)
            return validation.Fail();

        var user = await _repository.GetByEmail(request.Email, cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result.Fail(new Error("Invalid username or password."));

        string token = _jwtProvider.GenerateToken(user.Id.ToString(), user.FirstName, user.Email, user.IsAdmin);

        return Result.Ok(new LoginResponse(user.Id, user.Email, token));
    }
}