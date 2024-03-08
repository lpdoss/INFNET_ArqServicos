namespace Shared.RabbitMQ.Events;

public record ConfirmedStockEvent : IntegrationEvent
{
    public int OrderId {get; }

    public ConfirmedStockEvent(int orderId) 
    {
        OrderId = orderId;
    }
}