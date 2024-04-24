using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Interfaces;

namespace PortfolioManager.Api.Controllers;

[Authorize]
[Route("portfolio")]
public class PortfolioController(
    IPortfolioService service,
    IHttpContextAccessor context) : ApiController(context)
{
    private readonly IPortfolioService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<PortfolioProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPortfolio(GetPortfolioRequest request, CancellationToken ct)
    {
        var result = await _service.GetPortfolio(request, ct);

        return result.Match(x => Ok(x), Problem);
    }
}
