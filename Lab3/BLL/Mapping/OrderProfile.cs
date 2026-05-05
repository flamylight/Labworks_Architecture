using AutoMapper;
using Contracts.DTOs;
using DAL.Models;

namespace BLL.Mapping;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderService, OrderServiceItemDto>()
            .ForMember(dest => dest.Title,
                opt => 
                    opt.MapFrom(src => src.Service != null 
                    ? src.Service.Title 
                    : "Послуга видалена"))
            .ForMember(dest => dest.Price,
                opt => 
                    opt.MapFrom(src => src.Service != null 
                    ? src.Service.Price 
                    : 0));

        CreateMap<Order, GetOrderDto>()
            .ForMember(dest => dest.OrderServices, opt =>
                opt.MapFrom(src => src.OrderServices));
        
        CreateMap<CreateOrderDto, Order>();
    }
}