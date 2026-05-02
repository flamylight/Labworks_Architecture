using Contracts.DTOs;

namespace BLL.Interfaces;

public interface IOrderManager
{
    GetOrderDto CreateServiceOrder(CreateOrderDto dto);
    IEnumerable<GetOrderDto> GetAllOrders();
    void MarkAsDone(Guid orderId);
    GetOrderDto CreateTurnkeyOrder(CreateOrderDto dto);
    IEnumerable<GetOrderDto> GetPortfolioOrders();
    IEnumerable<GetOrderDto> GetDoneOrders();
    void MarkAsPortfolio(Guid orderId);
}