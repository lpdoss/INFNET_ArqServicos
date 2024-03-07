namespace Shared.RabbitMQ.Events;

public record ConfirmStockEvent : IntegrationEvent
{
    public int OrderId {get;}
    public List<OrderStockItem> OrderStockItems {get;}

    public ConfirmStockEvent(int orderId, List<OrderStockItem> orderStockItems) 
    {
        OrderId = orderId;
        OrderStockItems = orderStockItems;
    }
}

public record OrderStockItem
{
    public int ProductId {get;}
    public int Amount {get;}

    public OrderStockItem(int productId, int amount)
    {
        ProductId = productId;
        Amount = amount;
    }
}