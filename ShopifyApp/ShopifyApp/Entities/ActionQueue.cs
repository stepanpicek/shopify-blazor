using ShopifyApp.Core.Enums;
using ShopifyApp.Queues;

namespace ShopifyApp.Entities;

public class ActionQueue : IQueueItem
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsProcessing { get; set; }
    public DateTime? ProcessingStartedAt { get; set; }
    public DateTime RetryAt { get; set; }
    public int RetryCount { get; set; }
    public ActionType Type { get; set; }
    public string Template { get; set; }
    public string Parameters { get; set; }
}