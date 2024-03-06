using ShoppingCartService.Domain;
using Shared.Repository;

namespace ShoppingCartService.Repositories;

public interface ICartItemRepository : IRepository<CartItem>
{
    
}