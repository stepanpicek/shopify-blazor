using System.Linq.Expressions;

namespace ShopifyApp.Repositories;

public interface IRepository<TEntity> 
    where TEntity : class
{
    string TableName { get; }
    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> GetAllAsync();
    Task<TEntity> InsertAsync(TEntity entity);
    Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entities);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities);
    Task DeleteAsync(TEntity entity);
    Task DeleteAsync(IEnumerable<TEntity> entities);
    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);
    Task<long> CountAsync();
}