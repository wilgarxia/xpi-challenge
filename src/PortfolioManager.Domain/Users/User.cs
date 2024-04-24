using PortfolioManager.Domain.Common;

namespace PortfolioManager.Domain.Users;

public class User : Entity
{
    private User() { }

    private User(
        DateTime createdAt, 
        string firstName, 
        string lastName, 
        string email, 
        string passwordHash, 
        bool isAdmin = false, 
        Guid? id = null) : base(id ?? Guid.NewGuid()) 
    {
        CreatedAt = createdAt;
        FirstName = firstName;  
        Lastname = lastName;
        Email = email;
        PasswordHash = passwordHash;
        IsAdmin = isAdmin;  
    }

    public string FirstName { get; private set; } = null!;
    public string Lastname { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public bool IsAdmin { get; private set; } = false;
    public List<PortfolioProduct> PortfolioProducts { get; private set; } = [];
    public List<Transaction> Transactions { get; private set; } = [];

    public static User CreateAdmin(
            DateTime createdAt,
            string firstName,
            string lastName,
            string email,
            string passwordHash) =>
        new(createdAt, firstName, lastName, email, passwordHash, true);

    public static User CreateRegular(
            DateTime createdAt,
            string firstName,
            string lastName,
            string email,
            string passwordHash) =>
        new(createdAt, firstName, lastName, email, passwordHash);

    internal static User CreateForDataSeed(
            DateTime createdAt,
            string firstName,
            string lastName,
            string email,
            string passwordHash,
            bool isAdmin,
            Guid id) =>
        new(createdAt, firstName, lastName, email, passwordHash, isAdmin, id);

    public void AddProductToPortfolio(DateTime createdAt, Guid productId, Guid userId, decimal amount) 
    {
        if (PortfolioProducts.Any(x => x.ProductId == productId))
            return;

        PortfolioProducts.Add(PortfolioProduct.Create(createdAt, productId, userId, amount));
    }

    public void RemoveProductFromPortfolio(Guid productId)
    {
        if (!PortfolioProducts.Any(x => x.ProductId == productId))
            return;

        PortfolioProducts.Remove(PortfolioProducts.Single(x => x.ProductId == productId));
    }
}