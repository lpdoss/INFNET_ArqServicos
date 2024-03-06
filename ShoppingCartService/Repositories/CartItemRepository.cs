using ShoppingCartService.Domain;
using Shared.Repository;

namespace ShoppingCartService.Repositories;

public class CartItemRepository : UnitOfWork<CartItem>, ICartItemRepository
{
    public CartItemRepository(ShoppingCartDbContext context) : base(context)
    {
        
    }
}