namespace ShopifyApp.Settings;

public class ShopifySettings
{
    public string ClientId { get; set; } = "";
    public string ClientSecret { get; set; } = "";
    public IList<string> Scopes { get; set; } = new List<string>();
}