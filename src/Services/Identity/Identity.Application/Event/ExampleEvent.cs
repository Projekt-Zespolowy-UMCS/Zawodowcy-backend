using RabbitMQ.Events;

namespace Identity.Application.Event;

public record ExampleEvent : EventBusEvent
{        
    public int UserId { get; }
        
    public string UserName { get; }
        
    public ExampleEvent(int userId, string userName)
    {
        UserId = userId;
        UserName = userName;
    }

}
