using AutoMapper;
using Contracts.DTOs;
using DAL.Models;

namespace BLL.Mapping;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderService, OrderServiceItemDto>();

        CreateMap<Order, GetOrderDto>()
            .ForMember(dest => dest.OrderServices, opt =>
                opt.MapFrom(src => src.OrderServices));

        CreateMap<CreateOrderDto, Order>();
        
        CreateMap<CreateTurnkeyOrderDto, Order>()
            .ForMember(dest => dest.IsTurnkey, opt => 
                opt.MapFrom(src => true));
    }
}