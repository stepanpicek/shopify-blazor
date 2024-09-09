using Microsoft.AspNetCore.Identity;

namespace ShopifyApp.Entities;

public class ShopifyUser : IdentityUser
{
    public long ShopifyShopId { get; set; }
    public string ShopifyShopDomain { get; set; } = String.Empty;
    public long? ShopifyChargeId { get; set; }
    public DateTimeOffset? BillingOn { get; set; }
}