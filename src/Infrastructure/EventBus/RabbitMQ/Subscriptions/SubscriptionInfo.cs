namespace RabbitMQ.Subscriptions;

public record SubscriptionInfo
{
    public Type HandlerType { get; }

    public SubscriptionInfo(Type handlerType)
    {
        HandlerType = handlerType;
    }
}
