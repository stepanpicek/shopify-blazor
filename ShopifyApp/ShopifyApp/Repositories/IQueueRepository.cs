using ShopifyApp.Queues;

namespace ShopifyApp.Repositories;

public interface IQueueRepository<TQueueItem> where TQueueItem : class, IQueueItem
{
    Task<List<TQueueItem>> GetItemsAsync(int limit);
    Task<long> CountAsync();
    Task<long> CountProcessingAsync();
}