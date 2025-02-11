using System.ComponentModel.DataAnnotations.Schema;
using ShopifyApp.Queues;

namespace ShopifyApp.Entities;

[Table("action_queue_fatal")]
public class ActionQueueFatal : ActionQueue, IQueueItemFatal
{
    [Column("error_message")]
    public string ErrorMessage { get; set; }
}