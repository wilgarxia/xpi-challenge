using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Services;

namespace PortfolioManager.Api.Controllers;


[Route("tokens")]
[AllowAnonymous]
public class TokensController(ITokenService tokenService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateToken(
        [FromBody] CreateTokenRequest request, CancellationToken cancellationToken) =>
        await tokenService.CreateToken(request, cancellationToken) is var result && result.IsFailed
            ? Unauthorized(result.Value)
            : Ok(result.Value);
}