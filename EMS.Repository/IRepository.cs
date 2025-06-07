using EMS.Data;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EMS.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetById(int id);
        Task<List<T>> GetAll();
        Task<bool> isExists(int id);
        Task Add(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
        Task SaveChanges();
        Task<T> Find(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        Task<IList<T>> FindAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
    }
}
