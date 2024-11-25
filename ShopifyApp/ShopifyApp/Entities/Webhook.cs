using ShopifyApp.Core.Enums;

namespace ShopifyApp.Entities;

public class Webhook
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public WebhookType Type { get; set; }
    public string Payload { get; set; }
    public string Headers { get; set; }
    public string ShopifyUserId { get; set; }
}