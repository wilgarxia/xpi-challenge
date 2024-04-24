using PortfolioManager.Domain.Common;
using PortfolioManager.Domain.Products;

namespace PortfolioManager.Domain.Users;

public class PortfolioProduct : Entity
{
    private PortfolioProduct() { }

    private PortfolioProduct(
        DateTime createdAt,
        Guid userId,
        Guid productId,
        decimal amount,
        Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        CreatedAt = createdAt;
        UserId = userId;
        ProductId = productId;
        Amount = amount;
    }

    public DateTime? UpdatedAt { get; private set; }
    public User User { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public Product Product { get; private set; } = null!;
    public Guid ProductId { get; private set; }
    public decimal Amount { get; private set; }

    public static PortfolioProduct Create(DateTime createdAt, Guid productId, Guid userId, decimal amount) =>
        new(createdAt, userId, productId, amount);

    public void Increase(DateTime updatedAt, decimal amount)
    { 
        UpdatedAt = updatedAt;  
        Amount += amount;
    }

    public void Descrease(DateTime updatedAt, decimal amount)
    {
        UpdatedAt = updatedAt;
        Amount -= amount;
    }
}