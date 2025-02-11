using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopifyApp.Core.Enums;

namespace ShopifyApp.Entities;

[Table("webhook")]
public class Webhook
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("type")]
    public WebhookType Type { get; set; }
    
    [Column("payload")]
    public string? Payload { get; set; }
    
    [Column("headers")]
    public string? Headers { get; set; }
    
    [Column("shopify_user_id")]
    public string? ShopifyUserId { get; set; }
}