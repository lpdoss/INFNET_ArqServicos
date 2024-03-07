using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProductService.Domain;
using ProductService.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.RabbitMQ;
using Shared.RabbitMQ.Events;

namespace ProductService.Services;

public class RabbitMQConsumer_ConfirmStockEvent : IDisposable, IHostedService, IEventPublisher
{
    private readonly ILogger<RabbitMQConsumer_ConfirmStockEvent> _logger;
    private readonly IProductRepository _productRepository;
    private readonly string _queueName = "confirmStock";
    private IConnection _connection;
    private IModel _consumerChannel;

    public RabbitMQConsumer_ConfirmStockEvent(ILogger<RabbitMQConsumer_ConfirmStockEvent> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }
    public void Dispose()
    {
        _consumerChannel?.Dispose();
    }

    public async Task PublishAsync(IntegrationEvent @event, string queue)
    {
        _logger.LogInformation($"Start RabbitMQ publish {@event.GetType()}");

        if (!_connection.IsOpen) throw new Exception("Unable to connect to RabbitMQ");
        
        using var channel = _connection?.CreateModel() ?? throw new InvalidOperationException("RabbitMQ connection is not open");
        byte[] body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());
        
        channel.BasicPublish(exchange: string.Empty,
                             routingKey: queue,
                             basicProperties: null,
                             body: body);
        _logger.LogInformation($"Finish RabbitMQ publish {@event.GetType()}");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Factory.StartNew(() =>
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

                if (!_connection.IsOpen) return;
                
                // if (_createConsumer)
                _logger.LogInformation("Creating consumer channel");

                _consumerChannel = _connection.CreateModel();
                _consumerChannel.CallbackException += (sender, ea) => _logger.LogInformation(ea.Exception, "Error with RabbitMQ consumer channel");

                _consumerChannel.QueueDeclare(queue: _queueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                _logger.LogInformation("Creating basic consume");

                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
                consumer.Received += async (model, ea) => 
                {
                    try
                    {
                        var message = Encoding.UTF8.GetString(ea.Body.Span);
                        var @event = JsonSerializer.Deserialize<ConfirmStockEvent>(message);

                        var productsToUpdate = new List<Product>();
                        foreach (var orderStockItem in @event.OrderStockItems)
                        {
                            var product = await _productRepository.Get(orderStockItem.ProductId);

                            if (product.AmountInStock >= orderStockItem.Amount)
                            {
                                product.AmountInStock -= orderStockItem.Amount;
                                productsToUpdate.Add(product);
                            }
                            else
                            {
                                var noStockEvent = new NoStockEvent(@event.OrderId);
                                await PublishAsync(noStockEvent, "noStock");
                                return;
                            }
                        }
                        
                        foreach (var product in productsToUpdate)
                            await _productRepository.Update(product);
    
                        var confirmedStockEvent = new ConfirmedStockEvent(@event.OrderId);
                        await PublishAsync(confirmedStockEvent, "confirmedStock");
                    }
                    catch (System.Exception ex)
                    {
                        _logger.LogError(ex, "Error processing ConfirmStockEvent");
                    }
                };

                _consumerChannel.BasicConsume(
                        queue: _queueName,
                        autoAck: false,
                        consumer: consumer);
                _logger.LogInformation("Finish RabbitMQ connection on a background thread");
            
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting RabbitMQConnection");
            }
    
        }, TaskCreationOptions.LongRunning);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}