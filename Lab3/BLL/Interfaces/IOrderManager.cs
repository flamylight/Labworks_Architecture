using BLL.DTOs;

namespace BLL.Interfaces;

public interface IOrderManager
{
    GetOrderDto CreateOrder(CreateOrderDto dto);
}