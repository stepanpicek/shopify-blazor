using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopifyApp.Core.Enums;

namespace ShopifyApp.Entities;

[Table("trigger")]
public class Trigger
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    
    [Column("name")]
    public string? Name { get; set; }
    
    [Column("type")]
    public TriggerType Type { get; set; }
    
    [Column("is_active")]
    public bool IsActive { get; set; }
    
    [Column("conditions")]
    public string? Conditions { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("modified_at")]
    public DateTime ModifiedAt { get; set; }
    
    [Column("shopify_user_id")]
    public string? ShopifyUserId { get; set; }
}