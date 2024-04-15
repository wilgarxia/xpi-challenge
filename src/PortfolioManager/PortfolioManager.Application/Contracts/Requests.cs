namespace PortfolioManager.Application.Contracts;

public record CreateTokenRequest(string Username, string Password);
public record CreateUserRequest(string Username, string Password, bool IsAdmin);