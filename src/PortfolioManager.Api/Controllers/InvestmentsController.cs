using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Services;

namespace PortfolioManager.Api.Controllers;

[Authorize]
[Route("investments")]
public class InvestmentsController(IInvestmentService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllInvestments(GetAllInvestmentsRequest request, CancellationToken cancellationToken) =>
        await service.GetAll(request, cancellationToken) is var result && result.IsFailed
            ? BadRequest(result.Value)
            : Ok(result.Value);

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetInvestmentById(Guid id, CancellationToken cancellationToken) =>
        await service.GetById(id, cancellationToken) is var investment && investment.IsFailed
            ? BadRequest(investment.Value)
            : (investment.Value == null ? NotFound() : Ok(investment.Value));
}