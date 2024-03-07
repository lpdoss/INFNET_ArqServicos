using PaymentService.Domain;
using Shared.Repository;

namespace PaymentService.Repositories;

public class PaymentRepository : UnitOfWork<Payment>, IPaymentRepository
{
    public PaymentRepository(PaymentDbContext context) : base(context)
    {
        
    }
}