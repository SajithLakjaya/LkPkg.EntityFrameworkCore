using LkPkg.EntityFrameworkCore.Abstractions.Interfaces;
using LkPkg.EntityFrameworkCore.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LkPkg.EntityFrameworkCore.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Protected Members

        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        #endregion

        #region Constructor

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        #endregion

        #region IRepository<T> Members

        public virtual IQueryable<T> FindAll()
        {
            return _dbSet;
        }

        public virtual IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            Guard.IsNotNull(expression, nameof(expression));
            return _dbSet.Where(expression);
        }

        public virtual async Task<T?> FindByIdAsync(object id)
        {
            Guard.IsNotNull(id, nameof(id));
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T> InsertAsync(T entity)
        {
            Guard.IsNotNull(entity, nameof(entity));
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task<IEnumerable<T>> InsertRangeAsync(IEnumerable<T> entities)
        {
            Guard.IsNotNull(entities, nameof(entities));
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        public virtual T Update(T entity)
        {
            Guard.IsNotNull(entity, nameof(entity));
            _dbSet.Update(entity);
            return entity;
        }

        public virtual IEnumerable<T> UpdateRange(IEnumerable<T> entities)
        {
            Guard.IsNotNull(entities, nameof(entities));
            _dbSet.UpdateRange(entities);
            return entities;
        }

        public virtual void Remove(T entity)
        {
            Guard.IsNotNull(entity, nameof(entity));
            _dbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            Guard.IsNotNull(entities, nameof(entities));
            _dbSet.RemoveRange(entities);
        }

        #endregion

        #region IDisposable Members

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members
    }
}
