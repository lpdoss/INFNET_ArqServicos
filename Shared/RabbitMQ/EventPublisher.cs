using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Shared.RabbitMQ.Events;

namespace Shared.RabbitMQ;

public class EventPublisher : IEventPublisher
{
    private readonly ILogger<EventPublisher> _logger;
    private IConnection? _connection;

    public EventPublisher(ILogger<EventPublisher> logger)
    {
        _logger = logger;
    }
    
    public async Task PublishAsync(IntegrationEvent @event, string queue)
    {
        _logger.LogInformation($"Start RabbitMQ publish {@event.GetType()}");

        if (_connection == null)
            await GetOrCreateConnection();
        
        using var channel = _connection?.CreateModel() ?? throw new InvalidOperationException("RabbitMQ connection is not open");
        byte[] body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());
        
        channel.QueueDeclare(queue: queue,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

        channel.BasicPublish(exchange: string.Empty,
                             routingKey: queue,
                             basicProperties: null,
                             body: body);
        _logger.LogInformation($"Finish RabbitMQ publish {@event.GetType()}");
    }

    public Task GetOrCreateConnection()
    {
        try
        {
                _logger.LogInformation("Starting RabbitMQ connection on a background thread");
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest"
                };
                _connection = factory.CreateConnection();

            if (!_connection.IsOpen) throw new Exception("Unable to connect to RabbitMQ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting RabbitMQConnection");
        }
        return Task.CompletedTask;
    }
}