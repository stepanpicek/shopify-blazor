namespace ShopifyApp.Core.Settings;

public class WorkerSettings
{
    public bool Enabled { get; set; }
    public int Delay { get; set; }
    public int PrefetchCount { get; set; }
    public int ThreadCount { get; set; }
}