using Microsoft.EntityFrameworkCore;
using ShopifyApp.Core.Settings;
using ShopifyApp.Queues;
using ShopifyApp.Repositories;

namespace ShopifyApp.Workers;

public abstract class Worker<TDbContext, TEntity>(
    IQueueRepository<TEntity> repository,
    ILogger<Worker<TDbContext, TEntity>> logger,
    WorkerSettings settings)
    : IHostedService
    where TDbContext : DbContext
    where TEntity : class, IQueueItem
{
    protected readonly IQueueRepository<TEntity> Repository = repository;
    protected readonly ILogger<Worker<TDbContext, TEntity>> Logger = logger;
    protected readonly WorkerSettings Settings = settings;
    public Task? ExecuteTask { get; set; }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        ExecuteTask = ExecuteAsync(cancellationToken);

        if (ExecuteTask.IsCompleted)
        {
            return ExecuteTask;
        }
        
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if(ExecuteTask == null)
        {
            return;
        }
        
        //TODO: Implement graceful shutdown
    }
    
    private async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (!Settings.Enabled)
        {
            return;
        }
        
        var tasks = Enumerable.Range(0, settings.ThreadCount)
            .Select(i => Task.Run(() => RunWorkerTask(i, cancellationToken)))
            .ToList();

        await Task.WhenAll(tasks);
    }

    private async Task RunWorkerTask(int threadId, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var items = await Repository.GetItemsAsync(Settings.PrefetchCount);
                    if(!items.Any())
                    {
                        break;
                    }

                    foreach (var item in items)
                    {
                        try
                        {
                            await HandleItemAsync(item);
                        }
                        catch (Exception e)
                        {
                            Logger.LogError(e, "Error processing item");
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError(e, "Error fetching items");
                }
            }
            await Task.Delay(Settings.Delay * 1000, cancellationToken);
        }
    }
    
    protected abstract Task HandleItemAsync(TEntity item);
}