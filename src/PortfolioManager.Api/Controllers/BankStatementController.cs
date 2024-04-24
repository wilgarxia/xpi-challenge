using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Interfaces;

namespace PortfolioManager.Api.Controllers;

[Authorize]
[Route("bank-statement")]
public class BankStatementController(
    ITransactionService service,
    IHttpContextAccessor context) : ApiController(context)

{ 
    private readonly ITransactionService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<TransactionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBankStatement(GetAllTransactionsRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.GetAll(request, cancellationToken);

        return result.Match(x => Ok(x), Problem);
    }
}