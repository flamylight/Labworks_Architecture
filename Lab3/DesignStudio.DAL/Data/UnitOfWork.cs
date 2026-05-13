using DAL.Interfaces;
using DAL.Models;

namespace DAL.Data;

public class UnitOfWork: IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    
    public IGenericRepository<Service> Services { get; }
    public IGenericRepository<Order> Orders { get; }
    public IGenericRepository<OrderService> OrderServices { get; }
    public IGenericRepository<Package> Packages { get; }
    public IGenericRepository<PackageService> PackageServices { get; }

    public UnitOfWork(AppDbContext dbContext,
        IGenericRepository<Order> orders,
        IGenericRepository<Service> services,
        IGenericRepository<OrderService> orderServices,
        IGenericRepository<Package> packages,
        IGenericRepository<PackageService> packageServices)
    {
        _dbContext = dbContext;

        Orders = orders;
        Services = services;
        OrderServices = orderServices;
        Packages = packages;
        PackageServices = packageServices;
    }

    public void Save()
    {
        _dbContext.SaveChanges();
    }
}