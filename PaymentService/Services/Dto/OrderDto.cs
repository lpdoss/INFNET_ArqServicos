namespace PaymentService.Services.Dto;


public class OrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PaymentMethodId { get; set; }
    public string? PaymentMethod { get; set; }
    public int StatusId { get; set; }
    public string? Status { get; set; }

    public PaymentDto? Payment { get; set; }
    public IEnumerable<OrderItemDto> OrderItems { get; set; }
}