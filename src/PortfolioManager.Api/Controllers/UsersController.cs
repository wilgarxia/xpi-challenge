using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Interfaces;
using PortfolioManager.Infrastructure.Security.AuthorizationPolicies;

namespace PortfolioManager.Api.Controllers;

[Route("users")]
public class UsersController(
    IUserService userService, 
    IAuthenticationService tokenService,
    IHttpContextAccessor context) : ApiController(context)
{
    private readonly IUserService _userService = userService;
    private readonly IAuthenticationService _tokenService = tokenService;

    [HttpPost]
    [Authorize(Policy = AdminPolicyConfiguration.AdminUserPolicyName)]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.CreateUser(request, cancellationToken);

        return result.Match(x => Ok(x), Problem);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginUser(
        [FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _tokenService.Login(request, cancellationToken);

        return result.Match(x => Ok(x), Problem);
    }
}