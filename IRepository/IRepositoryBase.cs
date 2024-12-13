using System.Linq.Expressions;

namespace APIServerSmartHome.IRepository
{
    public interface IRepositoryBase<T> where T : class
    {
        Task AddAsync(T entity);
        Task UpdateAsync(int id,T entity);
        Task DeleteAsync(int id);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdEagerLoadingAsync(int id, Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllEagerLoadingAsync(Expression<Func<T, object>>[] includes);
        Task<int> CountAsync();

        Task<T> GetByNameAsync(string name);

    }
}
