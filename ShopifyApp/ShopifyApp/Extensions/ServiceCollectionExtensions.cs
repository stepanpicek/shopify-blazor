using Microsoft.EntityFrameworkCore;
using ShopifyApp.Settings;

namespace ShopifyApp.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgresDb<TDbContext>(this IServiceCollection services, PostgresSettings settings)
        where TDbContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(settings); 
        var connectionString = settings.ConnectionString;
        services.AddDbContext<TDbContext>(options => options.UseNpgsql(connectionString));
        return services;
    }
}