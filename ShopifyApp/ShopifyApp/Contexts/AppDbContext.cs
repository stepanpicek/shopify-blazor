using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopifyApp.Entities;

namespace ShopifyApp.Contexts;

public class AppDbContext : IdentityDbContext<ShopifyUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}