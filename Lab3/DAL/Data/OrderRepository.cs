using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public class OrderRepository(AppDbContext dbContext)
: GenericRepository<Order>(dbContext), IOrderRepository
{
    public override Order? GetById(Guid id)
    {
        return dbContext.Orders
            .Include(o => o.OrderServices)
            .ThenInclude(os => os.Service)
            .FirstOrDefault(o => o.Id == id);
    }

    public override IEnumerable<Order> GetAll()
    {
        return dbContext.Orders
            .Include(o => o.OrderServices)
            .ThenInclude(os => os.Service);
    }

    public IEnumerable<Order> GetPortfolioOrders()
    {
        return dbContext.Orders
            .Include(o => o.OrderServices)
            .ThenInclude(os => os.Service)
            .Where(o => o.IsDone && o.IsInPortfolio);
    }

    public IEnumerable<Order> GetDoneOrders()
    {
        return dbContext.Orders
            .Include(o => o.OrderServices)
            .ThenInclude(os => os.Service)
            .Where(o => o.IsDone);   
    }
}