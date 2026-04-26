using DAL.Models;

namespace DAL.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<Service> Services { get; }
    IGenericRepository<Order> Orders { get; }
    IGenericRepository<OrderService> OrderServices { get; }
    IGenericRepository<PortfolioItem> PortfolioItems { get; }

    void Save();
}