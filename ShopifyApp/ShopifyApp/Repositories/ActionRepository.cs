using Microsoft.EntityFrameworkCore;
using ShopifyApp.Contexts;
using Action = ShopifyApp.Entities.Action;

namespace ShopifyApp.Repositories;

public class ActionRepository(IDbContextFactory<AppDbContext> context)
    : Repository<AppDbContext, Action>(context), IActionRepository
{
}