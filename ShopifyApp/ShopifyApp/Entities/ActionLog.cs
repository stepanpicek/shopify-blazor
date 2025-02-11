using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopifyApp.Core.Enums;

namespace ShopifyApp.Entities;

[Table("action_log")]
public class ActionLog
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    
    [Column("action_id")]
    public long ActionId { get; set; }
    
    [Column("status")]
    public ActionStatus Status { get; set; }
    
    [Column("request")]
    public string? Request { get; set; }
    
    [Column("response")]
    public string? Response { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("completed_at")]
    public DateTime CompletedAt { get; set; }
}