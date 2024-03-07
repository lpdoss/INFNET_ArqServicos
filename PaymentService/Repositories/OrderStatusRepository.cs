using PaymentService.Domain;
using Shared.Repository;

namespace PaymentService.Repositories;

public class OrderStatusRepository : UnitOfWork<OrderStatus>, IOrderStatusRepository
{
    public OrderStatusRepository(PaymentDbContext context) : base(context)
    {
        
    }
}