using ShoppingCartService.Domain;
using ShoppingCartService.Services.Dto;

namespace ShoppingCartService.Services.Profile;

public class CartProfile : AutoMapper.Profile
{
    public CartProfile()
    {
        CreateMap<CartItem, CartItemDto>().ReverseMap();
        CreateMap<Cart, CartDto>()
            .ForMember(a => a.CartItems, a => a.MapFrom(b => b.CartItems));

        CreateMap<CartDto, Cart>()
            .ForMember(a => a.CartItems, a => a.MapFrom(b => b.CartItems));

    }
}