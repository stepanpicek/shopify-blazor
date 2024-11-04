using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ShopifyApp.Core.Services;

public class NavigationWrapper(NavigationManager navigationManager) : INavigationWrapper
{
    [JSInvokable]
    public void NavigateTo(string path)
    {
        navigationManager.NavigateTo(path);
    }
}