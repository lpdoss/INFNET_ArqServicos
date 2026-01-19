using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PaymentService.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.RabbitMQ;
using Shared.RabbitMQ.Events;

namespace PaymentService.Services;

public class RabbitMQConsumer_ConfirmedStockEvent : IDisposable, IHostedService, IEventPublisher
{
    private readonly ILogger<RabbitMQConsumer_ConfirmedStockEvent> _logger;
    private readonly IServiceProvider  _serviceProvider;
    private readonly string _queueName = "confirmedStock";
    private IConnection _connection;
    private IModel _consumerChannel;

    public RabbitMQConsumer_ConfirmedStockEvent(ILogger<RabbitMQConsumer_ConfirmedStockEvent> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
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

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Factory.StartNew(() =>
        {
            try
            {
                _logger.LogInformation("Starting RabbitMQ connection on a background thread");
                var factory = new ConnectionFactory()
                {
                    HostName = "10.106.13.47",
                    UserName = "default_user_3L9Ivs_uIVj2FpPS8qS",
                    Password = "APSzYWIn7CddEx57PF_PdXcuKz_EFGwT",
                    DispatchConsumersAsync = true
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
                        var @event = JsonSerializer.Deserialize<ConfirmedStockEvent>(message);
    
                        await using var scope = _serviceProvider.CreateAsyncScope();
                        var orderRepository = scope.ServiceProvider.GetService<IOrderRepository>();

                        var query = orderRepository.GetQueryable();
                        query = query.Include(a => a.OrderItems);
                        var order = await query.Where(a => a.Id == @event.OrderId).FirstAsync();
    
                        order.StatusId = 2; // PaymentApproved
                        order.Payment = new Domain.Payment()
                        {
                            OrderId = order.Id,
                            Financial = order.OrderItems.Sum(a => a.Amount * a.Price)
                        };
    
                        await orderRepository.Update(order);
    
                        var deleteCartEvent = new DeleteCartEvent(order.UserId, order.OrderItems.Select(a => a.ProductId).ToList());
                        await PublishAsync(deleteCartEvent, "deleteCart");

                        _consumerChannel.BasicAck(ea.DeliveryTag, multiple: false);
                    }
                    catch (System.Exception ex)
                    {
                        _logger.LogError(ex, "Error processing ConfirmedStockEvent");
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