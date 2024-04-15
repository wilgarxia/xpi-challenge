using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PortfolioManager.Api.Identity;
using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Services;

namespace PortfolioManager.Api.Controllers;

[Route("users")]
//[Authorize(Policy = PolicyConfiguration.AdminUserPolicyName)]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserRequest request, CancellationToken cancellationToken) =>
        await userService.CreateUser(request, cancellationToken) is var result && result.IsFailed
            ? BadRequest(result.Value)
            : (IActionResult)Ok(result.Value);
}
