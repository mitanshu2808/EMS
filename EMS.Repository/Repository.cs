using EMS.Data;
using EMS.Data.Entities.Leaves;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EMS.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly EmpDbContext _dbContext;
        private readonly DbSet<T> _db;

        public Repository(EmpDbContext context)
        {
            _dbContext = context;
            _db = _dbContext.Set<T>();
        }

        public async Task<T?> GetById(int id)
        {
            var data = await _db.FirstOrDefaultAsync(x => x.Id == id);
            return data;
        }

        public async Task<List<T>> GetAll()
        {
            return await _db.ToListAsync();
        }

        public async Task Add(T entity)
        {
            await _db.AddAsync(entity);
            await SaveChanges();
        }

        public async Task DeleteAsync(T entity)
        {
            _db.Remove(entity);
            await SaveChanges();
        }

        public async Task UpdateAsync(T entity)
        {
            _db.Update(entity);
            await SaveChanges();
        }

        public Task<bool> isExists(int id)
        {
            throw new NotImplementedException();
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> Find(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes != null)
            {
                query = includes(query);
            }

            return await query.FirstOrDefaultAsync(expression);

        }

        public async Task<IList<T>> FindAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            IQueryable<T> query = _db;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }
    }
}
