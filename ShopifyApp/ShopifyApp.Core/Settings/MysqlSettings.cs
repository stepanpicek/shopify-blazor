namespace ShopifyApp.Core.Settings;

public class MysqlSettings
{
    public string Host { get; set; } = "localhost";
    public string Database { get; set; } = "notifly";
    public string Username { get; set; } = "notifly";
    public string Password { get; set; } = "password";
    public string ConnectionString => $"Server={Host};Database={Database};Username={Username};Password={Password}";
}