using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Mvc;

namespace PortfolioManager.Application.Contracts;

public record LoginRequest(string Email, string Password);
public record CreateUserRequest(string FirstName, string LastName, string Email, string Password, bool IsAdmin);
public record GetPortfolioRequest([FromQuery] int PageIndex = 1, [FromQuery]int PageSize = 10);
public record GetAllProductsRequest([FromQuery]int PageIndex = 1, [FromQuery]int PageSize = 10);
public class CreateProductRequest
{
    public string Description { get; set; } = null!;
    public DateTime DueAt { get; set; }
    public string ManagerEmail { get; set; } = null!;
    public decimal MinimumInvestmentAmount { get; set; }
}

public class UpdateProductRequest
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public decimal MinimumInvestmentAmount { get; set; }
}

public record GetAllTransactionsRequest([FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 10);

public class AddTransactionRequest
{
    public Guid ProductId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
}

public class BuyOrSellProductRequest
{
    [FromBody]
    public decimal Amount { get; set; }
    [JsonIgnore]
    public Guid ProductId { get; set; }
}

public enum TransactionType
{
    Buy = 1,
    Sell
}