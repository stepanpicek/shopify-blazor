using ShopifyApp.Core.Enums;
using ShopifyApp.Queues;

namespace ShopifyApp.Entities;

public class TriggerQueue : IQueueItem
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsProcessing { get; set; }
    public DateTime? ProcessingStartedAt { get; set; }
    public DateTime RetryAt { get; set; }
    public int RetryCount { get; set; }
    public TriggerType Type { get; set; }
    public string Parameters { get; set; }
    public string ShopifyUserId { get; set; }
}