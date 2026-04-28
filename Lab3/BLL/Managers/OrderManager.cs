using BLL.DTOs;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Managers;

public class OrderManager(IUnitOfWork uow) : IOrderManager
{
    public GetOrderDto CreateServiceOrder(CreateOrderDto dto)
    {
        ValidateNewServiceOrder(dto);
        
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
    
    public IEnumerable<GetOrderDto> GetAllOrders()
    {
        var orders = uow.Orders.GetAll();
        
        return orders.Select(o => o.ToGetDto());
    }

    private void ValidateNewServiceOrder(CreateOrderDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new ArgumentException("Назва не може бути порожньою");
        }

        if (string.IsNullOrWhiteSpace(dto.ClientDescription))
        {
            throw new ArgumentException("Опис не може бути порожнім");
        }

        if (dto.ServiceIds.Count == 0 || dto.ServiceIds.Count > 1)
        {
            throw new ArgumentException("Замовлення має мати 1 послугу");
        }
    }
}