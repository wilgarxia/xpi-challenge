using PortfolioManager.Domain.Common;
using PortfolioManager.Domain.Products;

namespace PortfolioManager.Domain.Users;

public class User : Entity
{
    public string FirstName { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public required bool IsAdmin { get; set; } = false;
    public List<Product> Products { get; set; } = [];
}