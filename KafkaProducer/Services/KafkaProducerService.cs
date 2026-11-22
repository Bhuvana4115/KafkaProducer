using System.Text.Json;
using Confluent.Kafka;

namespace KafkaProducer.Services
{
    public interface IKafkaProducerService
    {
        Task ProduceAsync<T>(string key, T message, CancellationToken cancellationToken = default);
    }

    public sealed class KafkaProducerService : IKafkaProducerService, IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(IConfiguration configuration, ILogger<KafkaProducerService> logger)
        {
            _logger = logger;
            _topic = configuration["Kafka:Topic"] ?? throw new InvalidOperationException("Kafka:Topic is not configured.");

            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092",
                Acks = Acks.All,
                EnableIdempotence = true
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task ProduceAsync<T>(string key, T message, CancellationToken cancellationToken = default)
        {
            var result = await _producer.ProduceAsync(
                _topic,
                new Message<string, string>
                {
                    Key = key,
                    Value = JsonSerializer.Serialize(message)
                },
                cancellationToken);

            _logger.LogInformation("Produced message to {TopicPartitionOffset}", result.TopicPartitionOffset);
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(5));
            _producer.Dispose();
        }
    }
}
