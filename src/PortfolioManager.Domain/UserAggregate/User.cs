namespace PortfolioManager.Domain.UserAggregate;

public class User
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public required bool IsAdmin { get; set; } = false;
}