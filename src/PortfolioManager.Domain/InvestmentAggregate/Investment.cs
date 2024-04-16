using PortfolioManager.Domain.UserAggregate;

namespace PortfolioManager.Domain.InvestmentAggregate;

public class Investment
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
    public string Description { get; set; } = null!;
    public InvestmentType Type { get; set; }
    public Risk Risk { get; set; }
    public DateTime DueAt { get; set; }
    public decimal MinimumInvestmentAmount { get; set; }
    public bool IsAvailable { get; set; } = true;
    public User Manager { get; set; } = null!;
    public Guid UserId { get; set; }
}

public enum InvestmentType
{ 
    Bond,
    Fund,
}

public enum Risk
{
    Low,
    Medium,
    High
}