namespace ShopifyApp.Queues;

public interface IQueueItemFatal : IQueueItem
{
    string ErrorMessage { get; set; }
}