using ShopifyApp.Queues;

namespace ShopifyApp.Entities;

public class ActionQueueFatal : ActionQueue, IQueueItemFatal
{
    public string ErrorMessage { get; set; }
}