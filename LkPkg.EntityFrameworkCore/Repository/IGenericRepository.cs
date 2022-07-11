using System.Linq.Expressions;

namespace LkPkg.EntityFrameworkCore.Repository;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> FindAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    Task<T?> FindByIdAsync(object id);
    Task<T> InsertAsync(T entity);
    T Update(T entity);
    Task DeleteAsync(object id);
}