using Microsoft.EntityFrameworkCore;

namespace ShopifyApp.Extensions;

public static class DbContextExtensions
{
    public static string GetTableName<TEntity>(this DbContext context) where TEntity : class
    {
        var entityType = context.Model.FindEntityType(typeof(TEntity));
        return string.IsNullOrEmpty(entityType.GetSchema()) ? entityType.GetTableName() : $"{entityType.GetSchema()}.{entityType.GetTableName()}";
    }
    
}