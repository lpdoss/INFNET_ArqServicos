using ProductService.Services.Dto;

namespace ProductService.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAll();
    Task<ProductDto> Get(int id);
    Task Save(ProductDto user);
    Task Update(ProductDto user);
    Task Delete(ProductDto user);
}