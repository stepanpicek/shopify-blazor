using Microsoft.EntityFrameworkCore;
using ShopifyApp.Contexts;
using ShopifyApp.Entities;

namespace ShopifyApp.Repositories;

public class WebhookRepository(IDbContextFactory<AppDbContext> db)
    : Repository<AppDbContext, Webhook>(db), IWebhookRepository
{
}