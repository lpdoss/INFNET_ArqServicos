namespace ShoppingCartService.Domain;

public class Cart 
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public IEnumerable<CartItem> CartItems { get; set; }
}