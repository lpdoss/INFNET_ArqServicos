using PaymentService.Services.Dto;

namespace PaymentService.Services;

public interface IOrderService
{
    Task<List<OrderDto>> GetAll();
    Task<OrderDto> Get(int id);
    Task<OrderDto> GetByUser(int userId);
    Task Save(OrderDto user);
    Task Update(OrderDto user);
    Task Delete(OrderDto user);
}