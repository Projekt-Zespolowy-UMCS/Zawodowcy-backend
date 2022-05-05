using RabbitMQ.Events;
using RabbitMQ.Subscriptions;

namespace RabbitMQ;

public interface IEventBusSubscriptionManager
{
    bool IsEmpty { get; }
    void Clear();
    event EventHandler<string> OnEventRemoved;
    
    void AddSubscription<TEvent, THandler>()
        where TEvent : EventBusEvent
        where THandler : IEventBusEventHandler<TEvent>;

    void RemoveSubscription<TEvent, THandler>()
        where THandler : IEventBusEventHandler<TEvent>
        where TEvent : EventBusEvent;

    bool HasSubscriptionsForEvent<TEvent>() where TEvent : EventBusEvent;
    bool HasSubscriptionsForEvent(string eventName);
    Type GetEventTypeByName(string eventName);
    IEnumerable<SubscriptionInfo> GetHandlersForEvent<TEvent>() where TEvent : EventBusEvent;
    IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);
    string GetEventKey<TEvent>();
}
