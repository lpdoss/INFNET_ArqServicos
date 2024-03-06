namespace ShoppingCartService.Services.Dto;

public class CartItemDto
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Amount { get; set; }
    public bool IncludeInOrder { get; set; }
}