using ErrorOr;

using FluentValidation;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Interfaces;
using PortfolioManager.Domain.Users;
using PortfolioManager.Infrastructure.Common;
using PortfolioManager.Infrastructure.Security.PasswordHash;

namespace PortfolioManager.Application.Services;

public class UserService(
    IUserRepository repository,
    IPasswordHashProvider hashProvider,
    IValidator<CreateUserRequest> validator,
    IDateTimeProvider dateTimeProvider) : IUserService
{
    private readonly IUserRepository _repository = repository;
    private readonly IPasswordHashProvider _hashProvider = hashProvider;
    private readonly IValidator<CreateUserRequest> _validator = validator;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public async Task<ErrorOr<CreateUserResponse>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (await _validator.ValidateAsync(request, cancellationToken) is var validation && !validation.IsValid)
            return validation.Errors.ConvertAll(error => Error.Validation(error.PropertyName, error.ErrorMessage));

        if (await _repository.GetByEmail(request.Email, cancellationToken) is var userResult && userResult is not null)
            return UserFailures.UserAlreadyExists;

        var hashedPassword = _hashProvider.HashPassword(request.Password);

        User user = request.IsAdmin
            ? User.CreateAdmin(_dateTimeProvider.UtcNow, request.FirstName, request.LastName, request.Email, hashedPassword)
            : User.CreateRegular(_dateTimeProvider.UtcNow, request.FirstName, request.LastName, request.Email, hashedPassword);

        await _repository.Add(user, cancellationToken);

        CreateUserResponse result = new(user.Id, user.FirstName, user.Lastname, user.Email);

        return result;
    }
}