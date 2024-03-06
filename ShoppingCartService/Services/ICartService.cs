using ShoppingCartService.Services.Dto;

namespace ShoppingCartService.Services;

public interface ICartService
{
    Task<List<CartDto>> GetAll();
    Task<CartDto> Get(int id);
    Task<CartDto> GetByUser(int userId);
    Task Save(CartDto user);
    Task Update(CartDto user);
    Task Delete(CartDto user);
}