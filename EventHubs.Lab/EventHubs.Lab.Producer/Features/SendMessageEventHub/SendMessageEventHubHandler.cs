using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using MediatR;
using Microsoft.Extensions.Azure;
using System.Text;

namespace EventHubs.Lab.Producer.Features.SendMessageEventHub;

public class SendMessageEventHubHandler(
    ILogger<SendMessageEventHubHandler> logger, IAzureClientFactory<EventHubProducerClient> eventHubProducerClientFactory) : IRequestHandler<SendMessageEventHubCommand>
{
    public async Task Handle(SendMessageEventHubCommand command, CancellationToken cancellationToken)
    {
        var eventHubProducerClient = eventHubProducerClientFactory.CreateClient("EventHubProducerClient");

        var eventBatch = await eventHubProducerClient.CreateBatchAsync(
            new CreateBatchOptions { PartitionId = command.MessageRequest.PartitionId },
            cancellationToken);

        if (string.IsNullOrEmpty(command.MessageRequest.Message))
            throw new Exception("The Messages can not be null or empty");

        if (eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(command.MessageRequest.Message))))
            logger.LogInformation("Message added to batch");
        else
            logger.LogError("Message couldn't be addded");

        await eventHubProducerClient.SendAsync(eventBatch, cancellationToken);
    }
}
