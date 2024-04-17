using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PortfolioManager.Api.Extensions;
using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Services;
using PortfolioManager.Infrastructure.Security.AuthorizationPolicies;

namespace PortfolioManager.Api.Controllers;

[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _tokenService;

    public UsersController(IUserService userService, IAuthenticationService tokenService)
    {
        ArgumentNullException.ThrowIfNull(userService);
        ArgumentNullException.ThrowIfNull(tokenService);

        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost]
    [Authorize(Policy = AdminPolicyConfiguration.AdminUserPolicyName)]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserRequest request, CancellationToken cancellationToken) =>
        await _userService.CreateUser(request, cancellationToken) is var result && result.IsFailed
            ? BadRequest(result.ToProblem())
            : Ok(result.Value);

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(
        [FromBody] LoginRequest request, CancellationToken cancellationToken) =>
        await _tokenService.Login(request, cancellationToken) is var result && result.IsFailed
            ? BadRequest(result.ToProblem())
            : Ok(result.Value);
}
