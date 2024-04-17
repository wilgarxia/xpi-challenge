using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PortfolioManager.Api.Extensions;
using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Services;

namespace PortfolioManager.Api.Controllers;

[Authorize]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService _service;

    public ProductsController(IProductsService service)
    {
        ArgumentNullException.ThrowIfNull(service);

        _service = service;
    }

    [HttpGet]
    public IActionResult GetAllProducts(GetAllProductsRequest request) =>
        _service.GetAll(request) is var result && result.IsFailed
            ? BadRequest(result.ToProblem())
            : Ok(result.Value);

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductById(Guid id, CancellationToken cancellationToken) =>
        await _service.GetById(id, cancellationToken) is var product && product.IsFailed
            ? BadRequest(product.ToProblem())
            : (product.Value == null ? NotFound() : Ok(product.Value));

    [HttpPost]
    public async Task<IActionResult> AddProduct(AddProductRequest request, CancellationToken cancellationToken) =>
        await _service.AddProduct(request, cancellationToken) is var result && result.IsFailed
            ? BadRequest(result.ToProblem()) 
            : Ok();

    [HttpPut]
    public async Task<IActionResult> UpdateProduct(UpdateProductRequest request, CancellationToken cancellationToken) =>
        await _service.UpdateProduct(request, cancellationToken) is var result && result.IsFailed
            ? BadRequest(result.ToProblem())
            : (result.Value == null ? NotFound() : Ok(result.Value));
}