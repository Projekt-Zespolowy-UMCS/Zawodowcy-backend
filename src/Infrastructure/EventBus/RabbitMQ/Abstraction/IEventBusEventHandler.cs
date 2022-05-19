using RabbitMQ.Events;

namespace RabbitMQ;

public interface IEventBusEventHandler<in TEventBusEvent>
    where TEventBusEvent: EventBusEvent
{
    public Task Handle(TEventBusEvent @event);
}
