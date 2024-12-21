using APIServerSmartHome.Data;
using APIServerSmartHome.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;

namespace APIServerSmartHome.IRepository.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : BasicEntity<T>
    {
        protected readonly SmartHomeDbContext _dbContext;
        public RepositoryBase(SmartHomeDbContext context) 
        {
            _dbContext = context;        
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Set<T>().CountAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllEagerLoadingAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            foreach(var item in includes)
            {
                query = query.Include(item);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdEagerLoadingAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>().Where(x => x.Id == id);
            foreach(var item in includes)
            {
                query = query.Include(item);
            }
            return await query.FirstOrDefaultAsync();      
        }

        public async Task<T> GetByPropertyAsync(string property, string value)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(x => EF.Property<string>(x, property) == value);
        }

        public async Task UpdateAsync(int id, T entity)
        {
            var ex_entity = await _dbContext.Set<T>().FindAsync(id);
            if(ex_entity != null)
            {
                _dbContext.Update(ex_entity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
