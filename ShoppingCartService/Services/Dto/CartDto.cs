namespace ShoppingCartService.Services.Dto;


public class CartDto
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public IEnumerable<CartItemDto> CartItems { get; set; }
}