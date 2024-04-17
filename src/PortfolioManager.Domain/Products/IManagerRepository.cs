namespace PortfolioManager.Domain.Products;

public interface IManagerRepository
{
    Task<Manager?> GetByFirstNameAndLastName(string firstName, string lastName, CancellationToken ct);
    Task Add(Manager manager, CancellationToken cancellationToken);
}