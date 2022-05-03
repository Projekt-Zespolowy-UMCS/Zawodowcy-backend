using RabbitMQ.Client;

namespace RabbitMQ;

public interface IRabbitMQConnection: IDisposable
{
    public bool IsConnected { get; }

    public bool TryConnect();

    public IModel CreateModel();
}
