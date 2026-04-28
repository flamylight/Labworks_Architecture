namespace DAL.Interfaces;

public interface IGenericRepository<T> where T: class
{
    void Add(T entity);
    IEnumerable<T> GetAll();
}