using ErrorOr;

using PortfolioManager.Application.Contracts;

namespace PortfolioManager.Application.Interfaces;

public interface IAuthenticationService
{
    Task<ErrorOr<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken);
}