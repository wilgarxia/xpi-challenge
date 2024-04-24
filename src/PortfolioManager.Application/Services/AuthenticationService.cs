using ErrorOr;

using FluentValidation;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Interfaces;
using PortfolioManager.Domain.Users;
using PortfolioManager.Infrastructure.Security.Jwt;

namespace PortfolioManager.Application.Services;

public class AuthenticationService(
    IJwtTokenProvider jwtProvider, 
    IUserRepository repository, 
    IValidator<LoginRequest> validator) : IAuthenticationService
{
    private readonly IJwtTokenProvider _jwtProvider = jwtProvider;
    private readonly IUserRepository _repository = repository;
    private readonly IValidator<LoginRequest> _validator = validator;

    public async Task<ErrorOr<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        if (await _validator.ValidateAsync(request, cancellationToken) is var validation && !validation.IsValid)
            return validation.Errors.ConvertAll(error => Error.Validation(error.PropertyName, error.ErrorMessage));

        var user = await _repository.GetByEmail(request.Email, cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return UserFailures.InvalidUsernameOrPassword;

        string token = _jwtProvider.GenerateToken(user.Id.ToString(), user.FirstName, user.Email, user.IsAdmin);

        return new LoginResponse(user.Id, user.Email, token);
    }
}