using Confluent.Kafka;
using Consumer.Model;
using System.Text.Json;

namespace Consumer.Services
{
    public class ConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<ConsumerService> _logger;

        public ConsumerService(IConfiguration configuration, ILogger<ConsumerService> logger)
        {

            _logger = logger;
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "CanutoGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ProcessKafkaMessage(stoppingToken);
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        public ConsumerResponse ProcessKafkaMessage(CancellationToken cancellationToken)
        {
            try
            {
                _consumer.Subscribe("Canuto");

                var consume = _consumer.Consume(cancellationToken);
                var message = consume.Message.Value;

                _logger.LogInformation("Received data: {Msg}", message);

                _consumer.Close();

                return JsonSerializer.Deserialize<ConsumerResponse>(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing kafka message: {Ex}", ex.Message);
                return new ConsumerResponse();
            }
        }
    }
}