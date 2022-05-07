using Identity.Application.Event;
using RabbitMQ;

namespace Identity.Application.EventHandlers;

public class ExampleEventHandler: IEventBusEventHandler<ExampleEvent>
{
    public static bool IsReceived;
    private readonly ILogger<ExampleEventHandler> _logger;

    public ExampleEventHandler(ILogger<ExampleEventHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task Handle(ExampleEvent @event)
    {
        IsReceived = !IsReceived;
        _logger.LogCritical("Event happened lol");
    }
}
