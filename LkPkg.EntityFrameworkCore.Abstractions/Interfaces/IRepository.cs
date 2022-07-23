using System.Linq.Expressions;

namespace LkPkg.EntityFrameworkCore.Abstractions.Interfaces;

public interface IRepository<T> where T : class
{
    IQueryable<T> FindAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    Task<T?> FindByIdAsync(object id);
    Task<T> InsertAsync(T entity);
    Task<IEnumerable<T>> InsertRangeAsync(IEnumerable<T> entities);
    T Update(T entity);
    IEnumerable<T> UpdateRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}