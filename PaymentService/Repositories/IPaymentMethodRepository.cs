using PaymentService.Domain;
using Shared.Repository;

namespace PaymentService.Repositories;

public interface IPaymentMethodRepository : IRepository<PaymentMethod>
{
    
}