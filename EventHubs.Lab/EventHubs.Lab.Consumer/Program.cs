﻿// See https://aka.ms/new-console-template for more information
using Azure.Identity;
using EventHubs.Lab.Consumer;
using EventHubs.Lab.Consumer.Configuration;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var eventHubConfiguration = builder.Configuration.GetSection("EventHubConfiguration").Get<EventHubConfiguration>();

if (eventHubConfiguration == null)
    throw new ArgumentNullException(nameof(eventHubConfiguration));

builder.Services.AddAzureClients(factoryBuilder =>
{
    factoryBuilder.AddEventHubConsumerClientWithNamespace(eventHubConfiguration.ConsumerGroup,
            eventHubConfiguration.NameSpace, eventHubConfiguration.EventHubName)
        .WithName("EventHubConsumerClient")
        .WithCredential(new DefaultAzureCredential());
});

var host = builder.Build();
host.Run();
