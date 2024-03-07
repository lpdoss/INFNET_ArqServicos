using PaymentService.Domain;
using Shared.Repository;

namespace PaymentService.Repositories;

public class OrderRepository : UnitOfWork<Order>, IOrderRepository
{
    public OrderRepository(PaymentDbContext context) : base(context)
    {
        
    }
}