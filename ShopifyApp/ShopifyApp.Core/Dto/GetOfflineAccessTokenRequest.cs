using System.Text.Json.Serialization;

namespace ShopifyApp.Core.Dto;

public class GetOfflineAccessTokenRequest(string clientId, string clientSecret, string subjectToken)
{
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; } = clientId;
    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; set; } = clientSecret;
    [JsonPropertyName("grant_type")]
    public string GrantType { get; set; } = "urn:ietf:params:oauth:grant-type:token-exchange";
    [JsonPropertyName("subject_token")]
    public string SubjectToken { get; set; } = subjectToken;
    [JsonPropertyName("subject_token_type")]
    public string SubjectTokenType { get; set; } = "urn:ietf:params:oauth:token-type:id_token";
    [JsonPropertyName("requested_token_type")]
    public string RequestedTokenType { get; set; } = "urn:shopify:params:oauth:token-type:offline-access-token";
}