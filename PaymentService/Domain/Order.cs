namespace PaymentService.Domain;

public class Order 
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PaymentMethodId { get; set; }
    public int StatusId { get; set; }

    public PaymentMethod PaymentMethod { get; set; }
    public OrderStatus Status { get; set; }
    public Payment? Payment { get; set; }
    public IEnumerable<OrderItem> OrderItems { get; set; }
}