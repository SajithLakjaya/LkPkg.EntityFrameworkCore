using System.Linq.Expressions;
using LkPkg.EntityFrameworkCore.Core;
using LkPkg.EntityFrameworkCore.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LkPkg.EntityFrameworkCore.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> FindAll()
        {
            return _dbSet;
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            Guard.IsNotNull(expression, nameof(expression));
            return _dbSet.Where(expression);
        }

        public async Task<T?> FindByIdAsync(object id)
        {
            Guard.IsNotNull(id, nameof(id));
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> InsertAsync(T entity)
        {
            Guard.IsNotNull(entity, nameof(entity));
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public T Update(T entity)
        {
            Guard.IsNotNull(entity, nameof(entity));
            _dbSet.Update(entity);
            return entity;
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await FindByIdAsync(id);
            if (entity == null)
            {
                throw new RepositoryException($"Unable to find entity from id : {id}");
            }
            _dbSet.Remove(entity);
        }
    }
}
