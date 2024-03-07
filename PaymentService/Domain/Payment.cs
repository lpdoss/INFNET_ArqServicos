namespace PaymentService.Domain;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public double Financial { get; set; }
}