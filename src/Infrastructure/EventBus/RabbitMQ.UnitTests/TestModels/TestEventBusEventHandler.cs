using System.Threading.Tasks;

namespace RabbitMQ.UnitTests.TestModels;

public class TestEventBusEventHandler: IEventBusEventHandler<TestEventBusEvent>
{
    public Task Handle(TestEventBusEvent @event)
    {
        throw new System.NotImplementedException();
    }
}
