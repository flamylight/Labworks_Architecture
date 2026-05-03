using BLL.Interfaces;
using Contracts.DTOs;
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
            else
            {
                throw new ArgumentException("Сервіс не знайдено");
            }
        }
        
        uow.Orders.Add(orderEntity);
        uow.Save();

        var orderGetDto = orderEntity.ToGetDto();
        
        return orderGetDto;
    }
    
    public GetOrderDto CreateTurnkeyOrder(CreateOrderDto dto)
    {
        ValidateNewTurnkeyOrder(dto);
        
        var orderEntity = dto.ToEntity();
        var package = uow.Packages.GetById(orderEntity.PackageId!.Value);

        if (package == null)
        {
            throw new ArgumentException("Пакет послуг не знайдено");
        }

        foreach (var packageService in package.PackageServices)
        {
            if (packageService.Service != null)
            {
                orderEntity.OrderServices.Add(new OrderService
                {
                    ServiceId = packageService.ServiceId,
                    OrderId = orderEntity.Id
                });

                orderEntity.TotalPrice += packageService.Service.Price;
            }
            else
            {
                throw new ArgumentException("Сервіс не знайдено");
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
        
        return orders.Select(o => o.ToGetDto()).ToList();
    }

    public void MarkAsDone(Guid orderId)
    {
        var order = uow.Orders.GetById(orderId);

        if (order == null)
        {
            throw new ArgumentException("Замовлення не знайдено");
        }

        if (order.IsDone)
        {
            throw new ArgumentException("Замовлення вже виконано");
        }
        
        order.IsDone = true;
        order.FinishedAt = DateTime.UtcNow;
        
        uow.Orders.Update(order);
        uow.Save();
    }

    public IEnumerable<GetOrderDto> GetPortfolioOrders()
    {
        var orders = uow.Orders
            .QueryWithIncludes()
            .Where(o => o.IsInPortfolio && o.IsDone);
        
        return orders.Select(o => o.ToGetDto()).ToList();
    }

    public IEnumerable<GetOrderDto> GetDoneOrders()
    {
        var orders = uow.Orders
            .QueryWithIncludes()
            .Where(o => o.IsDone);
        
        return orders.Select(o => o.ToGetDto()).ToList();   
    }

    public void MarkAsPortfolio(Guid orderId)
    {
        var order = uow.Orders.GetById(orderId);

        if (order == null)
        {
            throw new ArgumentException("Такого замовлення не знайдено");
        }

        if (order.IsInPortfolio)
        {
            throw new ArgumentException("Замовлення вже є в портфоліо");
        }

        if (!order.IsDone)
        {
            throw new ArgumentException("В портфоліо можна додавати лише виконані замовлення");
        }
        
        order.IsInPortfolio = true;
        uow.Orders.Update(order);
        uow.Save();
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
    
    private void ValidateNewTurnkeyOrder(CreateOrderDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new ArgumentException("Назва не може бути порожньою");
        }

        if (string.IsNullOrWhiteSpace(dto.ClientDescription))
        {
            throw new ArgumentException("Опис не може бути порожнім");
        }

        if (dto.PackageId == null)
        {
            throw new ArgumentException("Не вибрано пакет послуг");
        }
    }
}