using PaymentService.Domain;
using Shared.Repository;

namespace PaymentService.Repositories;

public class OrderItemRepository : UnitOfWork<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(PaymentDbContext context) : base(context)
    {
        
    }
}