using ErrorOr;

using PortfolioManager.Application.Contracts;

namespace PortfolioManager.Application.Interfaces;

public interface IUserService
{
    Task<ErrorOr<CreateUserResponse>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken);
}