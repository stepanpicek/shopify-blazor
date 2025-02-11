using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopifyApp.Core.Enums;

namespace ShopifyApp.Entities;

[Table("action")]
public class Action
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    
    [Column("trigger_id")]
    public long TriggerId { get; set; }
    
    [Column("type")]
    public ActionType Type { get; set; }
    
    [Column("template")]
    public string? Template { get; set; }
}