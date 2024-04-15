using FluentResults;

namespace PortfolioManager.Domain.UserAggregate;

public class User
{
    public static Result<User> Create(string username, string password, bool isAdmin = false)
    {
        Result.FailIf(password.Length < 6, "Password must be at least 6 characters");

        User user = new()
        { 
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            IsAdmin = isAdmin
        };

        return Result.Ok(user);
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required bool IsAdmin { get; set; } = false;
}