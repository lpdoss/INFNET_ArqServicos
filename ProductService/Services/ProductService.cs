using ProductService.Domain;
using ProductService.Repositories;
using ProductService.Services.Dto;
using AutoMapper;

namespace ProductService.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    public async Task<List<ProductDto>> GetAll()
    {
        var result = await _productRepository.GetAll();
        return _mapper.Map<List<ProductDto>>(result);
    }
    public async Task<ProductDto> Get(int id)
    {
        var result = await _productRepository.Get(id);
        return _mapper.Map<ProductDto>(result);
    }
    public async Task Save(ProductDto productDto)
    {
        if (productDto.Id > 0)
            throw new ArgumentException("Product already has id.");
        var product = _mapper.Map<Product>(productDto);
        await _productRepository.Save(product);
    }

    public async Task Update(ProductDto productDto)
    {
        if (productDto.Id <= 0)
            throw new ArgumentException("Product has no id.");
        var product = _mapper.Map<Product>(productDto);
        await _productRepository.Update(product);
    }
    public async Task Delete(ProductDto productDto)
    {
        if (productDto.Id <= 0)
            throw new ArgumentException("Product is invalid.");
        var product = _mapper.Map<Product>(productDto);
        await _productRepository.Delete(product);
    }
}
