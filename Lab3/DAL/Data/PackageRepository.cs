using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public class PackageRepository(AppDbContext dbContext) 
    : GenericRepository<Package>(dbContext)
{
    public override IEnumerable<Package> GetAll()
    {
        return dbContext.Packages
            .Include(p => p.PackageServices)
            .ThenInclude(ps => ps.Service);
    }

    public override Package? GetById(Guid id)
    {
        return dbContext.Packages
            .Include(p => p.PackageServices)
            .ThenInclude(ps => ps.Service)
            .FirstOrDefault(p => p.Id == id);
    }
}