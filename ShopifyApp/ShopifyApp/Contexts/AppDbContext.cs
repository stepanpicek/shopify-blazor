using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopifyApp.Entities;
using Action = ShopifyApp.Entities.Action;

namespace ShopifyApp.Contexts;

public class AppDbContext : IdentityDbContext<ShopifyUser>
{
    public DbSet<Action> Actions { get; set; }
    public DbSet<ActionLog> ActionLogs { get; set; }
    public DbSet<ActionQueue> ActionQueues { get; set; }
    public DbSet<ActionQueueFatal> ActionQueueFatals { get; set; }
    public DbSet<Trigger> Triggers { get; set; }
    public DbSet<TriggerQueue> TriggerQueues { get; set; }
    public DbSet<TriggerQueueFatal> TriggerQueueFatals { get; set; }
    public DbSet<Webhook> Webhooks { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}