namespace eShop.BuildingBlocks.EventBus.Events;

public abstract class IntegrationEvent
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime CreationDate { get; private set; } = DateTime.UtcNow;
}
