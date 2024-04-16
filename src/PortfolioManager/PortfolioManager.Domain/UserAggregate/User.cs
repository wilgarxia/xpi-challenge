using NodaTime;

namespace PortfolioManager.Domain.UserAggregate;

public class User
{
    public static User Create(string username, string hashedPassword, bool isAdmin = false)
    {
        User user = new()
        { 
            Username = username,
            PasswordHash = hashedPassword,
            IsAdmin = isAdmin
        };

        return user;
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    public ZonedDateTime CreatedAt { get; set; } = SystemClock.Instance.GetCurrentInstant().InUtc();
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required bool IsAdmin { get; set; } = false;
}