using BLL.DTOs;

namespace BLL.Interfaces;

public interface IOrderManager
{
    GetOrderDto CreateServiceOrder(CreateOrderDto dto);
    IEnumerable<GetOrderDto> GetAllOrders();
    void MarkAsDone(Guid orderId);
}