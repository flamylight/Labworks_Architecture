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

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        
        Orders = new GenericRepository<Order>(dbContext);
        Services = new GenericRepository<Service>(dbContext);
        OrderServices = new GenericRepository<OrderService>(dbContext);
        PortfolioItems = new GenericRepository<PortfolioItem>(dbContext);
    }

    public void Save()
    {
        _dbContext.SaveChanges();
    }
}