using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ShopifyApp.Extensions;

namespace ShopifyApp.Repositories;

public abstract class Repository<TDbContext, TEntity>(IDbContextFactory<TDbContext> db) : IRepository<TEntity>
    where TEntity : class
    where TDbContext : DbContext
{
    private string _tableName;

    public string TableName
    {
        get
        {
            if (string.IsNullOrEmpty(_tableName))
            {
                var db = Db.CreateDbContext();
                _tableName = db.GetTableName<TEntity>();
            }
            return _tableName;
        }
    }

    protected readonly IDbContextFactory<TDbContext> Db = db;
    
    public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        using var db = Db.CreateDbContext();
        return db.Set<TEntity>()
            .Where(predicate)
            .ToListAsync();
    }

    public Task<List<TEntity>> GetAllAsync()
    {
        using var db = Db.CreateDbContext();
        return db.Set<TEntity>()
            .ToListAsync();
    }

    public Task<TEntity> InsertAsync(TEntity entity)
    {
        using var db = Db.CreateDbContext();
        db.Set<TEntity>().Add(entity);
        return db.SaveChangesAsync().ContinueWith(_ => entity);
    }

    public Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entities)
    {
        using var db = Db.CreateDbContext();
        db.Set<TEntity>().AddRange(entities);
        return db.SaveChangesAsync().ContinueWith(_ => entities);
    }

    public Task<TEntity> UpdateAsync(TEntity entity)
    {
        using var db = Db.CreateDbContext();
        db.Set<TEntity>().Update(entity);
        return db.SaveChangesAsync().ContinueWith(_ => entity);
    }

    public Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities)
    {
        using var db = Db.CreateDbContext();
        db.Set<TEntity>().UpdateRange(entities);
        return db.SaveChangesAsync().ContinueWith(_ => entities);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        await using var db = await Db.CreateDbContextAsync();
        db.Set<TEntity>().Remove(entity);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(IEnumerable<TEntity> entities)
    {
        await using var db = await Db.CreateDbContextAsync();
        db.Set<TEntity>().RemoveRange(entities);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        await using var db = await Db.CreateDbContextAsync();
        var entities = db.Set<TEntity>().Where(predicate);
        db.Set<TEntity>().RemoveRange(entities);
        await db.SaveChangesAsync();
    }

    public Task<long> CountAsync()
    {
        using var db = Db.CreateDbContext();
        return db.Set<TEntity>().LongCountAsync();
    }
}