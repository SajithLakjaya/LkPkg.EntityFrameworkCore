namespace LkPkg.EntityFrameworkCore.Abstractions.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;
        Task RollbackAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        bool HasTransaction();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
