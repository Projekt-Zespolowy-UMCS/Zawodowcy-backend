using Identity.Application.Event;
using Identity.Application.EventHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ;

namespace idsserver;

[Route("api/[controller]")]
[AllowAnonymous]
public class Test: ControllerBase
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<Test> _logger;
    public bool TestInt = true;

    public Test(IEventBus eventBus, ILogger<Test> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    [Route("event")]
    [HttpGet]
    public int Event() {
        _logger.LogInformation("Test endpoint hit :~DD");
        _eventBus.Publish(new ExampleEvent(1, "test"));
        if (ExampleEventHandler.IsReceived)
            return 1;
        return 2222;
    }
}
