namespace EventHubs.Lab.Producer.Configuration;

public record EventHubConfiguration
{
    public string? NameSpace { get; set; }
    public string? EventHubName { get; set; }
    public string? ConsumerGroup { get; set; }
}
