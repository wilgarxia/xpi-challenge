using PortfolioManager.Domain.Common;

namespace PortfolioManager.Domain.Products;

public class Product : Entity
{
    private Product() { }

    private Product(
        DateTime createdAt,
        string description,
        DateTime dueAt,
        decimal minimumInvestmentAmount,
        string managerEmail,
        Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        CreatedAt = createdAt;
        Description = description;
        DueAt = dueAt;
        MinimumInvestmentAmount = minimumInvestmentAmount;
        ManagerEmail = managerEmail;
        IsAvailable = true;
    }

    public DateTime? UpdatedAt { get; private set; }
    public string Description { get; private set; } = null!;
    public DateTime DueAt { get; private set; }
    public decimal MinimumInvestmentAmount { get; private set; }
    public bool IsAvailable { get; private set; } = true;
    public string ManagerEmail { get; private set; } = null!;

    public static Product Create(
            DateTime createdAt,
            string description,
            DateTime dueAt,
            decimal minimumInvestmentAmount,
            string managerEmail) =>
        new(createdAt, description, dueAt, minimumInvestmentAmount, managerEmail);

    private static Product CreateForDataSeed(
        DateTime createdAt,
        string description,
        DateTime dueAt,
        decimal minimumInvestmentAmount,
        string managerEmail,
        Guid id) =>
    new(createdAt, description, dueAt, minimumInvestmentAmount, managerEmail, id);

    public void Update(DateTime updatedAt, string description, decimal minimumInvestmentAmount)
    {
        UpdatedAt = updatedAt;
        Description = description;
        MinimumInvestmentAmount = minimumInvestmentAmount;
    }

    public void Deactivate(DateTime updatedAt)
    {
        UpdatedAt = updatedAt;
        IsAvailable = false;
    }
}