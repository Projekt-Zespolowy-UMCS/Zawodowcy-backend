using RabbitMQ.Events;

namespace RabbitMQ.Subscriptions;

public class InMemoryEventBusSubscriptionsManager: IEventBusSubscriptionManager
{
    private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
    private readonly List<Type> _eventTypes;

    public event EventHandler<string> OnEventRemoved;
    
    public bool IsEmpty => _handlers.Any();
    public void Clear() => _handlers.Clear();
    public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);
    public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(TEvent => TEvent.Name == eventName);
    public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];
    public string GetEventKey<TEvent>() => typeof(TEvent).Name;
    
    public InMemoryEventBusSubscriptionsManager()
    {
        _handlers = new Dictionary<string, List<SubscriptionInfo>>();
        _eventTypes = new List<Type>();
    }

    public void AddSubscription<TEvent, THandler>()
        where TEvent : EventBusEvent
        where THandler : IEventBusEventHandler<TEvent>
    {
        var eventName = GetEventKey<TEvent>();

        DoAddSubscription(typeof(THandler), eventName);

        if (!_eventTypes.Contains(typeof(TEvent)))
            _eventTypes.Add(typeof(TEvent));
    }

    private void DoAddSubscription(Type handlerType, string eventName)
    {
        if (!HasSubscriptionsForEvent(eventName))
            _handlers.Add(eventName, new List<SubscriptionInfo>());

        if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
            throw new ArgumentException(
                $"Handler Type {handlerType.Name} already registered for '{nameof(handlerType)}'");

        _handlers[eventName].Add(new SubscriptionInfo(handlerType));
    }

    public void RemoveSubscription<TEvent, THandler>()
        where THandler : IEventBusEventHandler<TEvent>
        where TEvent : EventBusEvent
    {
        var handlerToRemove = FindSubscriptionToRemove<TEvent, THandler>();
        var eventName = GetEventKey<TEvent>();
        DoRemoveHandler(eventName, handlerToRemove);
    }

    private void DoRemoveHandler(string eventName, SubscriptionInfo subsToRemove)
    {
        if (subsToRemove == null)
            return;
        
        _handlers[eventName].Remove(subsToRemove);
        if (_handlers[eventName].Any())
            return;
        
        _handlers.Remove(eventName);
        var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
        if (eventType != null)
            _eventTypes.Remove(eventType);
        RaiseOnEventRemoved(eventName);
    }

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent<TEvent>() where TEvent : EventBusEvent
    {
        var key = GetEventKey<TEvent>();
        return GetHandlersForEvent(key);
    }
    private void RaiseOnEventRemoved(string eventName)
    {
        var handler = OnEventRemoved;
        handler?.Invoke(this, eventName);
    }

    private SubscriptionInfo FindSubscriptionToRemove<TEvent, THandler>()
            where TEvent : EventBusEvent
            where THandler : IEventBusEventHandler<TEvent>
    {
        var eventName = GetEventKey<TEvent>();
        return DoFindSubscriptionToRemove(eventName, typeof(THandler));
    }

    private SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
    {
        if (!HasSubscriptionsForEvent(eventName))
            return null;

        return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
    }

    public bool HasSubscriptionsForEvent<TEvent>() where TEvent : EventBusEvent
    {
        var key = GetEventKey<TEvent>();
        return HasSubscriptionsForEvent(key);
    }
}
