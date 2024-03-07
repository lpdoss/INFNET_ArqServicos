namespace Shared.RabbitMQ.Events;

public record DeleteCartEvent : IntegrationEvent
{
    public int UserId {get;set;}
    public List<int> ProductIds {get;set;}

    public DeleteCartEvent(int userId, List<int> productIds) 
    {
        UserId = userId;
        ProductIds = productIds;
    }
}