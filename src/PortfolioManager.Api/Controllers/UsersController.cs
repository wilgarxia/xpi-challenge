using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Services;
using PortfolioManager.Infrastructure.Security.AuthorizationPolicies;

namespace PortfolioManager.Api.Controllers;

[Route("users")]
[Authorize(Policy = AdminPolicyConfiguration.AdminUserPolicyName)]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserRequest request, CancellationToken cancellationToken) =>
        await userService.CreateUser(request, cancellationToken) is var result && result.IsFailed
            ? BadRequest(result.Value)
            : Ok(result.Value);
}
