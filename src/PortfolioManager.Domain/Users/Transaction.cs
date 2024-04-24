using PortfolioManager.Domain.Common;
using PortfolioManager.Domain.Products;

namespace PortfolioManager.Domain.Users;

public class Transaction : Entity
{
    private Transaction() { }

    private Transaction(
        DateTime createdAt,
        string description,
        decimal amount,
        Guid userId,
        Guid productId,
        TransactionDirection direction,
        Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        CreatedAt = createdAt;
        Description = description;
        Amount = amount;
        UserId = userId;
        ProductId = productId;
        Direction = direction;
    }

    public string Description { get; private set; } = null!;
    public decimal Amount { get; private set; }
    public User User { get; private set; } = null!;
    public Product Product { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public Guid ProductId { get; private set; }
    public TransactionDirection Direction { get; private set; }

    public static Transaction CreateDebitTransaction(
        DateTime createdAt,
        string description,
        decimal amount,
        Guid userId,
        Guid productId) =>
    new(createdAt, description, amount, userId, productId, TransactionDirection.Debit);

    public static Transaction CreateCreditTransaction(
        DateTime createdAt,
        string description,
        decimal amount,
        Guid userId,
        Guid productId) =>
    new(createdAt, description, amount, userId, productId, TransactionDirection.Credit);
}

public enum TransactionDirection
{
    Debit = 1,
    Credit
}