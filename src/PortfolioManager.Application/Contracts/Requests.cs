using Microsoft.AspNetCore.Mvc;

namespace PortfolioManager.Application.Contracts;

public record LoginRequest(string Email, string Password);
public record CreateUserRequest(string FirstName, string LastName, string Email, string Password, bool IsAdmin);
public record GetAllProductsRequest([FromQuery]int PageIndex = 1, [FromQuery]int PageSize = 10);
public class AddProductRequest
{
    public string Description { get; set; } = null!;
    public DateTime DueAt { get; set; }
    public decimal MinimumInvestmentAmount { get; set; }
    public AddProductRequestManager Manager { get; set; } = null!;
}

public class AddProductRequestManager
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
}

public class UpdateProductRequest
{
    [FromQuery]
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public decimal MinimumInvestmentAmount { get; set; }
}