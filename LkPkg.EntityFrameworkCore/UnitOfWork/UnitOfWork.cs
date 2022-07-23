using LkPkg.EntityFrameworkCore.Abstractions.Interfaces;
using LkPkg.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Concurrent;
using System.Data;

namespace LkPkg.EntityFrameworkCore.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Private Members

        private readonly DbContext _context;
        private readonly ConcurrentDictionary<string, IRepository> _repositories;

        private IDbContextTransaction _transaction;

        #endregion 

        #region Constructor

        public UnitOfWork(DbContext context)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<string, IRepository>();
        }

        #endregion

        #region Public Methods

        public IRepository<T> Repository<T>() where T : class
        {
            return GetRepository<T>("generic");
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public bool HasTransaction() => _transaction != null;

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("There's already an active transaction.");
            }

            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                if (_transaction == null)
                {
                    throw new InvalidOperationException("There's no active transaction.");
                }

                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                DisposeTransaction();
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                await _transaction?.RollbackAsync();
            }
            catch
            {
            }
            finally
            {
                DisposeTransaction();
            }
        }

        #endregion

        #region Private Methods

        private void DisposeTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        private IRepository<T> GetRepository<T>(string prefix) where T: class
        {
            var type = typeof(T);
            var typeName = $"{prefix}.{type.FullName}";

            if (!_repositories.TryGetValue(typeName, out var repository))
            {
                repository = Factory<T>(_context);
                _repositories[typeName] = repository;
            }

            return (IRepository<T>)repository;
        }

        private IRepository Factory<T>(DbContext context) where T: class
        {
            return new Repository<T>(context);
        }

        #endregion

    }
}
