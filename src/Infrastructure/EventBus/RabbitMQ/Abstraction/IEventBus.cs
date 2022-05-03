using RabbitMQ.Events;

namespace RabbitMQ;

public interface IEventBus
{
    public void Publish(EventBusEvent @event);

    public void Subscribe<TEvent, THandler>()
        where TEvent : EventBusEvent
        where THandler : IEventBusEventHandler<TEvent>;

    public void Unsubscribe<TEvent, THandler>()
        where TEvent : EventBusEvent
        where THandler : IEventBusEventHandler<TEvent>;
}
