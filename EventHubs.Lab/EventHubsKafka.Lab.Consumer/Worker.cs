using Azure.Core;
using Azure.Identity;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EventHubsKafka.Lab.Consumer
{
    public class Worker(ILogger<Worker> logger, IConfiguration config) : BackgroundService
    {
        private static string? _principalName;
        private static string? _eventHubServiceUri;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _principalName = "c7687774-2052-4641-ac4d-2531f54a22e9";
            _eventHubServiceUri = "https://gap-lab-ns.servicebus.windows.net";

            var kafkaConfiguration = config.GetSection("ConsumerConfig").Get<ConsumerConfig>();


            //var conf = new ConsumerConfig
            //{
            //    GroupId = "gap-lab-kafka-cg",
            //    BootstrapServers = "gap-lab-ns.servicebus.windows.net:9093",
            //    SaslMechanism = Confluent.Kafka.SaslMechanism.OAuthBearer,
            //    SecurityProtocol = SecurityProtocol.SaslSsl,
            //    AutoOffsetReset = AutoOffsetReset.Latest
            //};

            using var kafkaConsumer = new ConsumerBuilder<string, byte[]>(kafkaConfiguration)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(Deserializers.ByteArray)
            .SetOAuthBearerTokenRefreshHandler((consumerRef, _) => TokenRefreshHandler(consumerRef))
            .Build();

            kafkaConsumer.Subscribe("gap-lab-hub");

            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Kafka Worker running at: {time}", DateTimeOffset.Now);
                var msg = kafkaConsumer.Consume(stoppingToken);

                logger.LogInformation("Event received, partition {partitionId}, Topic: {Topic}",
                    msg.Partition, msg.Topic);

                var test = Encoding.UTF8.GetString(msg.Message.Value);
                //var stream = new MemoryStream(msg.Message.Value);
                //var message = await JsonSerializer.DeserializeAsync<SendMessageEventHubRequest>(stream, cancellationToken: stoppingToken);

                logger.LogInformation($"Received: '{test}'");

                await Task.Delay(1000, stoppingToken);
            }
        }

        private static void TokenRefreshHandler(IClient kafkaClient)
        {
            var credentials = new DefaultAzureCredential();
            var request = new TokenRequestContext(new[] { _eventHubServiceUri });

            try
            {
                var token = credentials.GetToken(request);
                kafkaClient.OAuthBearerSetToken(token.Token, token.ExpiresOn.ToUnixTimeMilliseconds(), _principalName);
            }
            catch (Exception e)
            {
                kafkaClient.OAuthBearerSetTokenFailure(e.Message);
            }
        }
    }
}
