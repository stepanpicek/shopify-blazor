using ShopifyApp.Core.Enums;

namespace ShopifyApp.Entities;

public class ActionLog
{
    public long Id { get; set; }
    public long ActionId { get; set; }
    public ActionStatus Status { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime CompletedAt { get; set; }
}