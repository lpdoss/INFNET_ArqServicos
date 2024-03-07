using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ShoppingCartService.Domain;
using ShoppingCartService.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.RabbitMQ;
using Shared.RabbitMQ.Events;

namespace ShoppingCartService.Services;

public class RabbitMQConsumer_DeleteCartEvent : IDisposable, IHostedService
{
    private readonly ILogger<RabbitMQConsumer_DeleteCartEvent> _logger;
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly string _queueName = "deleteCart";
    private IConnection _connection;
    private IModel _consumerChannel;

    public RabbitMQConsumer_DeleteCartEvent(ILogger<RabbitMQConsumer_DeleteCartEvent> logger, ICartRepository cartRepository, ICartItemRepository cartItemRepository)
    {
        _logger = logger;
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
    }
    public void Dispose()
    {
        _consumerChannel?.Dispose();
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
                        var @event = JsonSerializer.Deserialize<DeleteCartEvent>(message);

                        var query = _cartRepository.GetQueryable();
                        query = query.Include(a => a.CartItems);

                        var cart = query.Where(a => a.UserId == @event.UserId).First();

                        // Some products not included in order, keep cart, just remove ordered products.
                        if (cart.CartItems.Any(a => !@event.ProductIds.Contains(a.ProductId)))
                        {
                            var cartItemsToRemove = cart.CartItems.Where(a => !@event.ProductIds.Contains(a.ProductId));
                            cart.CartItems = cart.CartItems.Except(cartItemsToRemove);

                            foreach(var cartItem in cartItemsToRemove)
                                await _cartItemRepository.Delete(cartItem);

                            await _cartRepository.Update(cart);
                            
                        }
                        // Else delete the cart
                        else 
                            await _cartRepository.Delete(cart);
                    }
                    catch (System.Exception ex)
                    {
                        _logger.LogError(ex, "Error processing DeleteCart");
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