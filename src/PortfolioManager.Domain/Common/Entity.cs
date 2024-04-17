namespace PortfolioManager.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public uint Version { get; init; }
}
