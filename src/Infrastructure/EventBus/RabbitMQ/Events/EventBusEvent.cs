using System.Text.Json.Serialization;

namespace RabbitMQ.Events;

public record EventBusEvent
{
    public Guid Id { get; private init; }
    public DateTime CreationDate { get; private init; }
    
    [JsonConstructor]
    public EventBusEvent(Guid id, DateTime createDate)
    {
        Id = id;
        CreationDate = createDate;
    }
    
    public EventBusEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.Now;
    }
}
