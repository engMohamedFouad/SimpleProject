using Microsoft.EntityFrameworkCore.Storage;
using SimpleProject.SharedRepositories;

namespace SimpleProject.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        public Task<IDbContextTransaction> BeginTransactionAsync();
        public Task CommitTransactionAsync();
        public Task RollbackTransactionAsync();
        Task<int> CompleteAsync();
    }
}
