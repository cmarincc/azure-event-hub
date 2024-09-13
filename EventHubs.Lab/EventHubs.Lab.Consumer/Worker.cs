using Azure.Messaging.EventHubs.Consumer;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHubs.Lab.Consumer;

internal class Worker(ILogger<Worker> logger, IAzureClientFactory<EventHubConsumerClient> eventHubConsumerClientFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var eventHubConsumerClient = eventHubConsumerClientFactory.CreateClient("EventHubConsumerClient");
        const string partitionId = "0";

        while (!stoppingToken.IsCancellationRequested) 
        {
            await foreach(var partitionEvent in eventHubConsumerClient.ReadEventsFromPartitionAsync(
                partitionId,
                EventPosition.Latest,
                stoppingToken)) 
            {
                logger.LogInformation("Event received, partition {partitionId}: {data}",
                    partitionEvent.Partition.PartitionId, Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray()));
                
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
