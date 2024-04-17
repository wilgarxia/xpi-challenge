using Microsoft.EntityFrameworkCore;

using PortfolioManager.Domain.Products;
using PortfolioManager.Infrastructure.Persistence.Commom;

namespace PortfolioManager.Infrastructure.Persistence.Products;

internal class ManagerRepository(AppDbContext context) : IManagerRepository
{
    public async Task<Manager?> GetByFirstNameAndLastName(string firstName, string lastName, CancellationToken ct) =>
        await context.Managers
            .Where(x => x.FirstName == firstName && x.Lastname == lastName)
        .FirstOrDefaultAsync(ct);

    public async Task Add(Manager manager, CancellationToken cancellationToken) =>
        await context.Managers.AddAsync(manager, cancellationToken);
}