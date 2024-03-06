using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Shared.Repository;

public interface IRepository<T>
{
    Task Save(T entity);
    Task Delete(T entity);
    Task Update(T entity);
    Task<T> Get(object id);
    Task<IEnumerable<T>> GetAll();
    Task<IDbContextTransaction> CreateTransaction();
    Task<IEnumerable<T>> GetAllByCriteria(Expression<Func<T, bool>> expression);
    Task<T> GetOneByCriteria(Expression<Func<T, bool>> expression);

    IQueryable<T> GetQueryable();
    IQueryable<T> GetQueryable(Expression<Func<T, bool>> expression);
}