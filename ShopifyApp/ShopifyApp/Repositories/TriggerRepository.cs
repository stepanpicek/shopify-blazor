using Microsoft.EntityFrameworkCore;
using ShopifyApp.Contexts;
using ShopifyApp.Entities;

namespace ShopifyApp.Repositories;

public class TriggerRepository(IDbContextFactory<AppDbContext> db)
    : Repository<AppDbContext, Trigger>(db), ITriggerRepository
{
}