namespace eShop.Basket.Infrastructure.EventBus;

public interface IEventBus
{
    void Publish<T>(string eventName, T message);
    void Subscribe<T>(string eventName, Action<T> handler);
}
