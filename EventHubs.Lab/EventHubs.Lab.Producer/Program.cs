using Azure.Identity;
using EventHubs.Lab.Producer.Configuration;
using EventHubs.Lab.Producer.Features.SendMessageEventHub;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var eventHubConfiguration = builder.Configuration.GetSection("EventHubConfiguration").Get<EventHubConfiguration>();

if (eventHubConfiguration == null )
   throw new ArgumentException(nameof(eventHubConfiguration));

builder.Services.AddAzureClients(factory =>
{
    factory.AddEventHubProducerClientWithNamespace(eventHubConfiguration.NameSpace, eventHubConfiguration.EventHubName)
    .WithName("EventHubProducerClient")
    .WithCredential(new DefaultAzureCredential());
});

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblies(typeof(SendMessageEventHubCommand).Assembly);
});

var app = builder.Build();

app.MapPost("/api/eventhub", async ([FromBody] SendMessageEventHubRequest request, IMediator mediator) =>
{
    await mediator.Send(new SendMessageEventHubCommand(request));
    return Results.Ok();
});

app.MapGet("/", () => "Hello World!");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
