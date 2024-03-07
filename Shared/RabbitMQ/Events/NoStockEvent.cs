namespace Shared.RabbitMQ.Events;

public record NoStockEvent : IntegrationEvent
{
    public int OrderId {get;set;}

    public NoStockEvent(int orderId) 
    {
        OrderId = OrderId;
    }
}