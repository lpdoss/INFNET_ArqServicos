using ShoppingCartService.Domain;
using Shared.Repository;

namespace ShoppingCartService.Repositories;

public class CartRepository : UnitOfWork<Cart>, ICartRepository
{
    public CartRepository(ShoppingCartDbContext context) : base(context)
    {
        
    }
}