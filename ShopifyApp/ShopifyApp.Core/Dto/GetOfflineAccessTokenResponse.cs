using System.Text.Json.Serialization;

namespace ShopifyApp.Core.Dto;

public class GetOfflineAccessTokenResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
    
    [JsonPropertyName("scope")]
    public string? Scope { get; set; }
}