using DAL.Interfaces;
using DAL.Models;

namespace DAL.Data;

public class UnitOfWork: IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    
    public IGenericRepository<Service> Services { get; }
    public IGenericRepository<Order> Orders { get; }
    public IGenericRepository<OrderService> OrderServices { get; }
    public IGenericRepository<PortfolioItem> PortfolioItems { get; }
    public IGenericRepository<Package> Packages { get; }
    public IGenericRepository<PackageService> PackageServices { get; }

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        
        Orders = new OrderRepository(dbContext);
        Services = new GenericRepository<Service>(dbContext);
        OrderServices = new GenericRepository<OrderService>(dbContext);
        PortfolioItems = new GenericRepository<PortfolioItem>(dbContext);
        Packages = new PackageRepository(dbContext);
        PackageServices = new GenericRepository<PackageService>(dbContext);
    }

    public void Save()
    {
        _dbContext.SaveChanges();
    }
}