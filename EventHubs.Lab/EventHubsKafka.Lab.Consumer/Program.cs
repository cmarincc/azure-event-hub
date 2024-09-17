using Confluent.Kafka;
using EventHubsKafka.Lab.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();


var host = builder.Build();
host.Run();
