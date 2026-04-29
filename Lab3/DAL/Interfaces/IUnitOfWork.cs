using DAL.Models;

namespace DAL.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<Service> Services { get; }
    IOrderRepository Orders { get; }
    IGenericRepository<OrderService> OrderServices { get; }
    IGenericRepository<Package> Packages { get; }
    IGenericRepository<PackageService> PackageServices { get; }

    void Save();
}