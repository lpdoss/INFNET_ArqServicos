using PaymentService.Services.Dto;
using PaymentService.Domain;

namespace PaymentService.Services.Profile;

public class OrderProfile : AutoMapper.Profile
{
    public OrderProfile()
    {
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        CreateMap<Payment, PaymentDto>().ReverseMap();
        CreateMap<Order, OrderDto>()
            .ForMember(a => a.PaymentMethod, a => a.MapFrom(b => b.PaymentMethod.Name))
            .ForMember(a => a.Status, a => a.MapFrom(b => b.Status.Name))
            .ForMember(a => a.OrderItems, a => a.MapFrom(b => b.OrderItems));

        CreateMap<OrderDto, Order>();

    }
}