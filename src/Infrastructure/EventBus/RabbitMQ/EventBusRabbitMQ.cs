using RabbitMQ.Events;

namespace RabbitMQ;

public class EventBusRabbitMQ: IEventBus, IDisposable
{
    public void Publish(EventBusEvent @event)
    {
        throw new NotImplementedException();
    }

    public void Subscribe<TEvent, THandler>() where TEvent : EventBusEvent where THandler : IEventBusEventHandler<TEvent>
    {
        throw new NotImplementedException();
    }

    public void Unsubscribe<TEvent, THandler>() where TEvent : EventBusEvent where THandler : IEventBusEventHandler<TEvent>
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
