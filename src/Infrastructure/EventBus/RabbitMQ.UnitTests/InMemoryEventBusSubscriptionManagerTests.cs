using RabbitMQ.Subscriptions;
using RabbitMQ.UnitTests.TestModels;
using Xunit;

namespace RabbitMQ.UnitTests;

public class InMemoryEventBusSubscriptionManagerTests
{
    [Fact]
    public void Should_add_event_to_subscription_manager()
    {
        var manager = new InMemoryEventBusSubscriptionsManager();
        manager.AddSubscription<TestEventBusEvent, TestEventBusEventHandler>();

        var hasEvent = manager.HasSubscriptionsForEvent<TestEventBusEvent>();
        
        Assert.True(hasEvent);
    }
    
    [Fact]
    public void Should_remove_subscription_from_subscription_manager()
    {
        var manager = new InMemoryEventBusSubscriptionsManager();
        manager.AddSubscription<TestEventBusEvent, TestEventBusEventHandler>();
        manager.RemoveSubscription<TestEventBusEvent, TestEventBusEventHandler>();

        var hasEvent = manager.HasSubscriptionsForEvent<TestEventBusEvent>();
        
        Assert.False(hasEvent);
    }
}
