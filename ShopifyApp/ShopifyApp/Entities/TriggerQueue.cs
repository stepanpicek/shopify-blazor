using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopifyApp.Core.Enums;
using ShopifyApp.Queues;

namespace ShopifyApp.Entities;

[Table("trigger_queue")]
public class TriggerQueue : IQueueItem
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("is_processing")]
    public bool IsProcessing { get; set; }
    
    [Column("processing_started_at")]
    public DateTime? ProcessingStartedAt { get; set; }
    
    [Column("retry_at")]
    public DateTime RetryAt { get; set; }
    
    [Column("retry_count")]
    public int RetryCount { get; set; }
    
    [Column("type")]
    public TriggerType Type { get; set; }
    
    [Column("parameters")]
    public string? Parameters { get; set; }
    
    [Column("shopify_user_id")]
    public string? ShopifyUserId { get; set; }
}