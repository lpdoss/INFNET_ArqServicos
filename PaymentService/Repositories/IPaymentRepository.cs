using PaymentService.Domain;
using Shared.Repository;

namespace PaymentService.Repositories;

public interface IPaymentRepository : IRepository<Payment>
{
    
}