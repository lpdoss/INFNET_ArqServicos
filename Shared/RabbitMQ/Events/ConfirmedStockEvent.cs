namespace Shared.RabbitMQ.Events;

public record ConfirmedStockEvent : IntegrationEvent
{
    public int OrderId {get;set;}

    public ConfirmedStockEvent(int orderId) 
    {
        OrderId = OrderId;
    }
}