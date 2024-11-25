using ShopifyApp.Queues;

namespace ShopifyApp.Entities;

public class TriggerQueueFatal : TriggerQueue, IQueueItemFatal
{
    public string ErrorMessage { get; set; }
}