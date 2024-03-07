using Shared.RabbitMQ.Events;

namespace Shared.RabbitMQ;

public interface IEventPublisher
{
    Task PublishAsync(IntegrationEvent @event, string queue);
}