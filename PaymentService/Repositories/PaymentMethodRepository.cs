using PaymentService.Domain;
using Shared.Repository;

namespace PaymentService.Repositories;

public class PaymentMethodRepository : UnitOfWork<PaymentMethod>, IPaymentMethodRepository
{
    public PaymentMethodRepository(PaymentDbContext context) : base(context)
    {
        
    }
}