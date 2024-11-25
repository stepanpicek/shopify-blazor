using ShopifyApp.Core.Enums;

namespace ShopifyApp.Entities;

public class Action
{
    public long Id { get; set; }
    public long TriggerId { get; set; }
    public ActionType Type { get; set; }
    public string? Template { get; set; }
}