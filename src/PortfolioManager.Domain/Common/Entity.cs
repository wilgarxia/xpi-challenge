namespace PortfolioManager.Domain.Common;

public abstract class Entity
{
    protected Entity(Guid id)
    {
        Id = id;
    }

    protected Entity() { }

    public Guid Id { get; private init; }
    public DateTime CreatedAt { get; init; }
    public uint Version { get; init; }
}
