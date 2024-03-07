namespace PaymentService.Domain;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Amount { get; set; }
    public double Price { get; set; }

}
