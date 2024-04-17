using PortfolioManager.Domain.Common;
using PortfolioManager.Domain.Products;
using PortfolioManager.Domain.Users;

namespace PortfolioManager.Domain.Transactions;

public class Transaction : Entity
{
    public string Description { get; set; } = null!;
    public decimal Amount { get; set; }
    public User User { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public TransactionDirection Direction { get; set; }

}

public enum TransactionDirection
{ 
    Debit = 1,
    Credit
}