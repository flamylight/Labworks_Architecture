using DAL.Interfaces;

namespace DAL.Data;

public class GenericRepository<T>(AppDbContext dbContext) 
    : IGenericRepository<T> where T : class
{
    public virtual void Add(T entity)
    {
        dbContext.Set<T>().Add(entity);
    }

    public virtual IEnumerable<T> GetAll()
    {
        return dbContext.Set<T>().ToList();
    }

    public virtual T? GetById(Guid id)
    {
        return dbContext.Set<T>().Find(id);
    }

    public virtual void Update(T entity)
    {
        dbContext.Set<T>().Update(entity);
    }

    public virtual IQueryable<T> QueryWithIncludes()
    {
        return dbContext.Set<T>();
    }
}