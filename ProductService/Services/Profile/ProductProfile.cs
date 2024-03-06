using ProductService.Domain;
using ProductService.Services.Dto;

namespace ProductService.Services.Profile;

public class ProductProfile : AutoMapper.Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}