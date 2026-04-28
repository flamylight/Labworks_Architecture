using DAL.Interfaces;

namespace DAL.Data;

public class GenericRepository<T>(AppDbContext dbContext) 
    : IGenericRepository<T> where T : class
{
    public void Add(T entity)
    {
        dbContext.Set<T>().Add(entity);
    }

    public IEnumerable<T> GetAll()
    {
        return dbContext.Set<T>().ToList();
    }
}