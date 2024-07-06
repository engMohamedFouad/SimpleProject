using Microsoft.EntityFrameworkCore.Storage;

namespace SimpleProject.SharedRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        public IQueryable<T> GetAsQueryable();
        public Task<List<T>> GetListAsync();
        public Task<T> GetByIdAsync(int id);
        public Task AddAsync(T entity);
        public Task Updatesync(T entity);
        public Task Deletesync(T entity);
        public Task AddRangeAsync(IEnumerable<T> entities);
        public Task DeleteRangeAsync(IEnumerable<T> entities);
        public Task UpdateRangeAsync(IEnumerable<T> entities);
        public Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
