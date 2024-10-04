namespace ShopifyApp.Settings;

public class PostgresSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5432;
    public string Database { get; set; } = "shopify";
    public string Username { get; set; } = "postgres";
    public string Password { get; set; } = "postgres";
    public string ConnectionString => $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password}";
}