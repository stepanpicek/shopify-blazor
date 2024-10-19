namespace ShopifyApp.Core.Services;

public interface IShopService
{
    Task GetShopAsync(string shop);
}