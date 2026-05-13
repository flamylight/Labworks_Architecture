using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using Contracts.DTOs;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Managers;

public class OrderManager(IUnitOfWork uow, IMapper mapper) : IOrderManager
{
    public GetOrderDto CreateServiceOrder(CreateOrderDto dto)
    {
        ValidateNewServiceOrder(dto);
        
        var orderEntity = mapper.Map<Order>(dto);

        var service = uow.Services.GetById(dto.ServiceId);

        if (service == null)
        {
            throw new NotFoundException("Сервіс не знайдено");
        }
        
        orderEntity.OrderServices.Add(new OrderService
        {
            OrderId = orderEntity.Id,
            Title = service.Title,
            Description = service.Description,
            Price = service.Price
        });
        
        
        orderEntity.TotalPrice = orderEntity.OrderServices.Sum(os => os.Price);
        
        uow.Orders.Add(orderEntity);
        uow.Save();
        
        return mapper.Map<GetOrderDto>(orderEntity);
    }
    
    public GetOrderDto CreateTurnkeyOrder(CreateTurnkeyOrderDto dto)
    {
        ValidateNewTurnkeyOrder(dto);
        
        var orderEntity = mapper.Map<Order>(dto);
        var package = uow.Packages.GetById(dto.PackageId!.Value);

        if (package == null)
        {
            throw new NotFoundException("Пакет послуг не знайдено");
        }

        foreach (var packageService in package.PackageServices)
        {
            if (packageService.Service != null)
            {
                orderEntity.OrderServices.Add(new OrderService
                {
                    OrderId = orderEntity.Id,
                    Title = packageService.Service.Title,
                    Description = packageService.Service.Description,
                    Price = packageService.Service.Price
                });
            }
            else
            {
                throw new NotFoundException("Сервіс не знайдено");
            }
        }
        
        orderEntity.TotalPrice = orderEntity.OrderServices.Sum(os => os.Price);       
        
        uow.Orders.Add(orderEntity);
        uow.Save();
        
        return mapper.Map<GetOrderDto>(orderEntity);
    }
    
    public IEnumerable<GetOrderDto> GetAllOrders()
    {
        var orders = uow.Orders.GetAll();
        
        return orders.Select(o => mapper.Map<GetOrderDto>(o)).ToList();
    }

    public void MarkAsDone(Guid orderId)
    {
        var order = uow.Orders.GetById(orderId);

        if (order == null)
        {
            throw new NotFoundException("Замовлення не знайдено");
        }

        if (order.IsDone)
        {
            throw new BadRequestException("Замовлення вже виконано");
        }
        
        order.IsDone = true;
        order.FinishedAt = DateTime.UtcNow;
        
        uow.Orders.Update(order);
        uow.Save();
    }

    public IEnumerable<GetOrderDto> GetPortfolioOrders()
    {
        return uow.Orders
            .QueryWithIncludes()
            .Where(o => o.IsInPortfolio && o.IsDone)
            .Select(o => mapper.Map<GetOrderDto>(o))
            .ToList();
    }

    public IEnumerable<GetOrderDto> GetDoneOrders()
    {
        return uow.Orders
            .QueryWithIncludes()
            .Where(o => o.IsDone)
            .Select(o => mapper.Map<GetOrderDto>(o))
            .ToList(); 
    }

    public void MarkAsPortfolio(Guid orderId)
    {
        var order = uow.Orders.GetById(orderId);

        if (order == null)
        {
            throw new BadRequestException("Такого замовлення не знайдено");
        }

        if (order.IsInPortfolio)
        {
            throw new BadRequestException("Замовлення вже є в портфоліо");
        }

        if (!order.IsDone)
        {
            throw new BadRequestException("В портфоліо можна додавати лише виконані замовлення");
        }
        
        order.IsInPortfolio = true;
        uow.Orders.Update(order);
        uow.Save();
    }

    public void DeleteOrder(Guid orderId)
    {
        var order = uow.Orders.GetById(orderId);

        if (order == null)
        {
            throw new NotFoundException("Замовлення не знайдено!");
        }

        if (!order.IsDone)
        {
            throw new BadRequestException("Не можна видалити не виконане замовлення!");
        }
        
        uow.Orders.Delete(order);
        uow.Save();       
    }
    
    private void ValidateNewServiceOrder(CreateOrderDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new BadRequestException("Назва не може бути порожньою");
        }

        if (string.IsNullOrWhiteSpace(dto.ClientDescription))
        {
            throw new BadRequestException("Опис не може бути порожнім");
        }
    }
    
    private void ValidateNewTurnkeyOrder(CreateTurnkeyOrderDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new BadRequestException("Назва не може бути порожньою");
        }

        if (string.IsNullOrWhiteSpace(dto.ClientDescription))
        {
            throw new BadRequestException("Опис не може бути порожнім");
        }

        if (dto.PackageId == null)
        {
            throw new BadRequestException("Не вибрано пакет послуг");
        }
    }
}