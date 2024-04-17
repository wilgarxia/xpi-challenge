using PortfolioManager.Domain.Common;

namespace PortfolioManager.Domain.Products;

public class Manager : Entity
{
    public static Manager Create(string firstName, string lastName, string email)
    { 
        return new Manager() { FirstName = firstName, Lastname = lastName, Email = email };    
    }

    public string FirstName { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<Product> Products { get; set; } = [];
}
