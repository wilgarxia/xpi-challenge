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