namespace PortfolioManager.Infrastructure.Security;

public interface IPasswordHashProvider
{
    string HashPassword(string password);
}

public class PasswordHashProvider : IPasswordHashProvider
{
    public string HashPassword(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password);
}
