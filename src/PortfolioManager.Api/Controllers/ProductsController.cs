using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Interfaces;
using PortfolioManager.Infrastructure.Security.AuthorizationPolicies;

namespace PortfolioManager.Api.Controllers;

[Authorize]
[Route("products")]
public class ProductsController(
    IProductsService service, 
    IHttpContextAccessor context) : ApiController(context)
{
    private readonly IProductsService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<ProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllProducts(GetAllProductsRequest request, CancellationToken ct)
    {
        var result = await _service.GetAll(request, ct);

        return result.Match(x => Ok(x), Problem);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetById(id, cancellationToken);

        return result.Match(x => Ok(x), Problem);
    }

    [HttpPost]
    [Authorize(Policy = AdminPolicyConfiguration.AdminUserPolicyName)]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateProduct(request, cancellationToken);

        return result.Match(x => 
            Created(Url.Action("GetProductById", new { id = x.Id }), x), 
            Problem);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = AdminPolicyConfiguration.AdminUserPolicyName)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        request.Id = id;

        var result = await _service.UpdateProduct(request, cancellationToken);

        return result.Match(x => NoContent(), Problem);
    }

    [HttpPatch("{id:guid}/deactivate")]
    [Authorize(Policy = AdminPolicyConfiguration.AdminUserPolicyName)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeactivateProduct(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.DeactivateProduct(id, cancellationToken);

        return result.Match(x => NoContent(), Problem);
    }

    [HttpPost("{id:guid}/buy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]    
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> BuyProduct(
        [FromRoute] Guid id, [FromBody] BuyOrSellProductRequest request, CancellationToken cancellationToken)
    {
        request.ProductId = id;

        var result = await _service.BuyProduct(request, cancellationToken);

        return result.Match(x => NoContent(), Problem);
    }

    [HttpPost("{id:guid}/sell")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SellProduct(
        [FromRoute] Guid id, [FromBody] BuyOrSellProductRequest request, CancellationToken cancellationToken)
    {
        request.ProductId = id;

        var result = await _service.SellProduct(request, cancellationToken);

        return result.Match(x => NoContent(), Problem);
    }       
}