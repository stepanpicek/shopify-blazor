using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopifyApp.Entities;

namespace ShopifyApp.Contexts;

public class PostgresDbContext : IdentityDbContext<ShopifyUser>
{
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
    }
}