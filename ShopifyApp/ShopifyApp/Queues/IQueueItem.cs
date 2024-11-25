namespace ShopifyApp.Queues;

public interface IQueueItem
{
    long Id { get; set; }
    DateTime CreatedAt { get; set; }
    bool IsProcessing { get; set; }
    DateTime? ProcessingStartedAt { get; set; }
    DateTime RetryAt { get; set; }
    int RetryCount { get; set; }
}