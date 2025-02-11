using ShopifyApp.Contexts;
using ShopifyApp.Core.Settings;
using ShopifyApp.Entities;
using ShopifyApp.Repositories;

namespace ShopifyApp.Workers;

public class ActionQueueWorker(
    IQueueRepository<ActionQueue> repository,
    ILogger<Worker<AppDbContext, ActionQueue>> logger,
    WorkerSettings settings) : Worker<AppDbContext, ActionQueue>(repository, logger, settings)
{
    protected override Task HandleItemAsync(ActionQueue item)
    {
        //TODO: Implement action handling
        throw new NotImplementedException();
    }
}