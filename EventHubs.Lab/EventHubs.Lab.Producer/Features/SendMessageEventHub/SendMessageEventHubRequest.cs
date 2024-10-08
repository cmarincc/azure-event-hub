﻿namespace EventHubs.Lab.Producer.Features.SendMessageEventHub;

public record SendMessageEventHubRequest
{
    public string? Message { get; set; }
    public string? Messager { get; set; }
    public DateTime SentAt { get; set; }
    public string? PartitionId { get; set; }
}
