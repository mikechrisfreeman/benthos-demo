using Confluent.Kafka;
using System.Text.Json;

namespace Kafka.Api
{
    public interface IKafkaService
    {
        Task RaiseEvent(string topic, Todo todo);
    }

    public class KafkaService : IKafkaService
    {
        private readonly ProducerConfig _config;
        
        public KafkaService(string bootstrapServers)
        {
            _config = new ProducerConfig { BootstrapServers = bootstrapServers };
        }

        public async Task RaiseEvent(string topic, Todo todo)
        {
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                var delivery = await producer.ProduceAsync(topic, new Message<Null, string> { Value = JsonSerializer.Serialize(todo)});

                if (delivery.Status != PersistenceStatus.Persisted)
                    throw new Exception($"delivery not persisted, {delivery.Message}");
            }
        }

    }
}
