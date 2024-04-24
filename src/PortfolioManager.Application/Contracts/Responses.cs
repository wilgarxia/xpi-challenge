namespace PortfolioManager.Application.Contracts;

public record LoginResponse(Guid Id, string Email, string Token);
public record CreateUserResponse(Guid Id, string FirstName, string LastName, string Email);
public record ProductResponse(
    Guid Id, 
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string Description,
    DateTime DueAt,
    decimal MinimumInvestmentAmount);

public record PortfolioProductResponse(
    ProductResponse Product,
    decimal Amount);

public class TransactionResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Operation { get; set; }
    public TransactionProduct Product { get; set; } = null!;

    public class TransactionProduct
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = null!;
    }
}