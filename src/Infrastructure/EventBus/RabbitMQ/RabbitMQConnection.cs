using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace RabbitMQ;

public class RabbitMQConnection: IRabbitMQConnection
{
    private IConnectionFactory _connectionFactory;
    private ILogger<RabbitMQConnection> _logger;
    private readonly int _retryCount;
    private bool _disposed;
    private IConnection _connection;

    public bool IsConnected
    {
        get => _connection != null && _connection.IsOpen && !_disposed;
    }

    public RabbitMQConnection(IConnectionFactory connectionFactory, ILogger<RabbitMQConnection> logger, int retryCount = 5)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentException("Connection factory cannot be null.");
        _logger = logger ?? throw new ArgumentException("Logger cannot be null.");
        _retryCount = retryCount;
    }

    public IModel CreateModel()
    {
        if (!IsConnected)
            throw new InvalidOperationException("You cannot perform that action without available RabbitMQ connection.");

        return _connection.CreateModel();
    }
    
    public void Dispose()
    {
        if (_disposed) return;

        try
        {
            _connection.ConnectionShutdown -= OnConnectionShutdown;
            _connection.ConnectionBlocked -= OnConnectionBlocked;
            _connection.CallbackException -= OnCallbackException;
            _connection.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogCritical("FATAL ERROR: Could not dispose RabbitMQ connection. Is it created and opened?");
        }
    }

    public bool TryConnect()
    {
        _logger.LogInformation("Trying to connect to RabbitMQ event bus.");

        var policy = RetryPolicy.Handle<SocketException>()
            .Or<BrokerUnreachableException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(5), (ex, time) =>
            {
                _logger.LogWarning($"Could not connect to RabbitMQ event bus. Trying to reconnect. [${time.Seconds}, ${ex.Message}]");
            });
        
        policy.Execute(() =>
        {
            _connection = _connectionFactory.CreateConnection();
        });
        
        if (IsConnected)
        {
            _connection.ConnectionShutdown += OnConnectionShutdown;
            _connection.CallbackException += OnCallbackException;
            _connection.ConnectionBlocked += OnConnectionBlocked;
            
            _logger.LogInformation($"RabbitMQ Client acquired a persistent connection to '${_connection.Endpoint.HostName}' and has subscribed to failure events");
            return true;
        }
        else
        {
            _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");
            return false;
        }
    }

    private void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
    {
        if (_disposed) return;
        
        _logger.LogWarning($"A RabbitMQ connection was shutdown. Trying to reconnect...");
        
        TryConnect();
    }

    private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
    {
        if (_disposed) return;
        
        _logger.LogWarning($"A RabbitMQ connection was blocked. Trying to reconnect...");
        
        TryConnect();
    }

    private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
    {
        if (_disposed) return;
        
        _logger.LogWarning($"There was a RabbitMQ callback exception. Trying to reconnect...");
        
        TryConnect();
    }
}
