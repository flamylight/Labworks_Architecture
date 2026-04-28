using BLL.DTOs;
using DAL.Models;

namespace BLL.Mappers;

public static class OrderMapper
{
    public static GetOrderDto ToGetDto(this Order entity)
    {
        return new GetOrderDto
        {
            Id = entity.Id,
            Title = entity.Title,
            CreatedAt = entity.CreatedAt,
            TotalPrice = entity.TotalPrice,
            IsTurnkey = entity.IsTurnkey,
            IsDone = entity.IsDone,
            ClientDescription = entity.ClientDescription,
            FinishedAt = entity.FinishedAt,
            OrderServices = entity.OrderServices.Select(os => new OrderServiceItemDto
            {
                Title = os.Service?.Title ?? "Послуга видалена",
                Price = os.Service?.Price ?? 0
            }).ToList()
        };
    }

    public static Order ToEntity(this CreateOrderDto dto)
    {
        return new Order
        {
            Title = dto.Title,
            ClientDescription = dto.ClientDescription,
            IsTurnkey = dto.IsTurnkey,
            PackageId = dto.PackageId
        };
    }
}