namespace PortfolioManager.Domain.Users;

public interface IPortfolioProductRepository
{
    IQueryable<PortfolioProduct> GetPortfolioQueryForPagination(Guid userId);
}
