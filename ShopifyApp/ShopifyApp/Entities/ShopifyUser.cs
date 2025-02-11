using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ShopifyApp.Entities;

public class ShopifyUser : IdentityUser
{
    [Column("shopify_shop_id")]
    public long ShopifyShopId { get; set; }
    
    [Column("shopify_shop_domain")]
    public string? ShopifyShopDomain { get; set; }
    
    [Column("shopify_charge_id")]
    public long? ShopifyChargeId { get; set; }
    
    [Column("billing_on")]
    public DateTimeOffset? BillingOn { get; set; }
}