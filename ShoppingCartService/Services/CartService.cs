using ShoppingCartService.Domain;
using ShoppingCartService.Repositories;
using ShoppingCartService.Services.Dto;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCartService.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public CartService(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }
    public async Task<List<CartDto>> GetAll()
    {
        var query = _cartRepository.GetQueryable();
        query = query.Include(a => a.CartItems);
        var result = query.AsEnumerable();
        if (result.Any())
            return _mapper.Map<List<CartDto>>(result);
        return null;
    }
    public async Task<CartDto> Get(int id)
    {
        var query = _cartRepository.GetQueryable();
        query = query.Include(a => a.CartItems);

        var result = query.Where(a => a.Id == id).FirstOrDefault();
        if (result != null)
            return _mapper.Map<CartDto>(result);
        return null;
    }
    public async Task<CartDto> GetByUser(int userId)
    {
        var query = _cartRepository.GetQueryable();
        query = query.Include(a => a.CartItems);

        var result = query.Where(a => a.UserId == userId).FirstOrDefault();
        if (result != null)
            return _mapper.Map<CartDto>(result);
        return null;
    }
    public async Task Save(CartDto cartDto)
    {
        if (cartDto.Id > 0)
            throw new ArgumentException("Cart already has id.");
        var cart = _mapper.Map<Cart>(cartDto);
        await _cartRepository.Save(cart);
    }

    public async Task Update(CartDto cartDto)
    {
        if (cartDto.Id <= 0)
            throw new ArgumentException("Cart has no id.");
        var cart = _mapper.Map<Cart>(cartDto);
        await _cartRepository.Update(cart);
    }
    public async Task Delete(CartDto cartDto)
    {
        if (cartDto.Id <= 0)
            throw new ArgumentException("Cart is invalid.");
        var cart = _mapper.Map<Cart>(cartDto);
        await _cartRepository.Delete(cart);
    }
}
