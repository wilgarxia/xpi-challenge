using ErrorOr;

namespace PortfolioManager.Domain.Users;

public static class UserFailures
{
    public static readonly Error UserAlreadyExists = Error.Validation("usu-0001", "User already exists.");
    public static readonly Error InvalidUsernameOrPassword = Error.Validation("usu-0002", "Invalid Username or Password.");
}
