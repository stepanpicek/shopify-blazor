using Microsoft.EntityFrameworkCore;
using ShopifyApp.Core.Settings;

namespace ShopifyApp.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMySqlDb<TDbContext>(this IServiceCollection services, MysqlSettings settings)
        where TDbContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(settings); 
        var connectionString = settings.ConnectionString;
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 36));
        services.AddDbContextFactory<TDbContext>(options => options.UseMySql(connectionString, serverVersion));
        return services;
    }
}