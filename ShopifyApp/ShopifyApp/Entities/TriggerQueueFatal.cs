using System.ComponentModel.DataAnnotations.Schema;
using ShopifyApp.Queues;

namespace ShopifyApp.Entities;

[Table("trigger_queue_fatal")]
public class TriggerQueueFatal : TriggerQueue, IQueueItemFatal
{
    [Column("error_message")]
    public string? ErrorMessage { get; set; }
}