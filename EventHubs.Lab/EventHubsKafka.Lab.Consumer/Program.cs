using Confluent.Kafka;
using EventHubsKafka.Lab.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection("ConsumerConfig"));

var host = builder.Build();
host.Run();
