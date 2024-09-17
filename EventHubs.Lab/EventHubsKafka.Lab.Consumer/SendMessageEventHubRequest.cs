namespace EventHubsKafka.Lab.Consumer;

public class SendMessageEventHubRequest
{
    public string? Message { get; set; }
    public string? Messager { get; set; }
    public DateTime SentAt { get; set; }
    public string? PartitionId { get; set; }
}
