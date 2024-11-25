using ShopifyApp.Core.Enums;

namespace ShopifyApp.Entities;

public class Trigger
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public TriggerType Type { get; set; }
    public bool IsActive { get; set; }
    public string Conditions { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public string ShopifyUserId { get; set; }
}