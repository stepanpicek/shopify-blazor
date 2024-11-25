using ShopifyApp.Queues;

namespace ShopifyApp.Repositories;

public interface IQueueFatalRepository<TQueueItem> : IQueueRepository<TQueueItem>
    where TQueueItem : class, IQueueItemFatal
{
    
}