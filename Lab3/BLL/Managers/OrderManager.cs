using BLL.DTOs;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Managers;

public class OrderManager(IUnitOfWork uow) : IOrderManager
{
    public GetOrderDto CreateOrder(CreateOrderDto dto)
    {
        var orderEntity = dto.ToEntity();

        foreach (Guid serviceId in dto.ServiceIds)
        {
            var service = uow.Services.GetById(serviceId);

            if (service != null)
            {
                orderEntity.OrderServices.Add(new OrderService
                {
                    ServiceId = serviceId,
                    OrderId = orderEntity.Id
                });

                orderEntity.TotalPrice += service.Price;
            }
        }
        
        uow.Orders.Add(orderEntity);
        uow.Save();

        var orderGetDto = orderEntity.ToGetDto();
        
        return orderGetDto;
    }
}