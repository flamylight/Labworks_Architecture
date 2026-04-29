using DAL.Models;

namespace DAL.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    IEnumerable<Order> GetPortfolioOrders();
    IEnumerable<Order> GetDoneOrders();
}