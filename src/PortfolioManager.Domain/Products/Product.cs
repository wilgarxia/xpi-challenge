using PortfolioManager.Domain.Common;
using PortfolioManager.Domain.Users;

namespace PortfolioManager.Domain.Products;

public class Product : Entity
{
    public DateTime? UpdatedAt { get; set; }
    public string Description { get; set; } = null!;
    public DateTime DueAt { get; set; }
    public decimal MinimumInvestmentAmount { get; set; }
    public bool IsAvailable { get; set; } = true;
    public Manager Manager { get; set; } = null!;
    public Guid ManagerId { get; set; }
    public List<User> Users { get; set; } = [];
}