using FluentResults;

using FluentValidation;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Extensions;
using PortfolioManager.Domain.Users;
using PortfolioManager.Infrastructure.Persistence.Commom;
using PortfolioManager.Infrastructure.Security.PasswordHash;

namespace PortfolioManager.Application.Services;

public interface IUserService
{
    Task<Result<CreateUserResponse>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken);
}

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IUserRepository _repository;
    private readonly IPasswordHashProvider _hashProvider;
    private readonly IValidator<CreateUserRequest> _validator;
    public UserService(
        AppDbContext context,
        IUserRepository repository, 
        IPasswordHashProvider hashProvider, 
        IValidator<CreateUserRequest> validator)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(hashProvider);
        ArgumentNullException.ThrowIfNull(validator);

        _context = context;
        _repository = repository;
        _hashProvider = hashProvider;
        _validator = validator;
    }

    public async Task<Result<CreateUserResponse>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (await _validator.ValidateAsync(request, cancellationToken) is var validation && !validation.IsValid)
            return validation.Fail();

        if (await _repository.GetByEmail(request.Email, cancellationToken) is var user && user is not null)
            return Result.Fail("User already exists");
        
        user = new User()
        { 
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            FirstName = request.FirstName,
            Lastname = request.LastName,
            Email = request.Email,
            PasswordHash = _hashProvider.HashPassword(request.Password),
            IsAdmin = request.IsAdmin
        };

        await _repository.Add(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        CreateUserResponse result = new(user.Id, user.FirstName, user.Lastname, user.Email);

        return Result.Ok(result);
    }
}