using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using DotNet46ApiExample.Models;
using Newtonsoft.Json;

namespace DotNet46ApiExample.Services
{
    public class KafkaMessageProducer : IKafkaMessageProducer
    {
        private readonly string _bootstrapServers;
        private readonly string _topic;

        public KafkaMessageProducer()
            : this(ConfigurationManager.AppSettings["KafkaBootstrapServers"], ConfigurationManager.AppSettings["KafkaTopic"])
        {
        }

        public KafkaMessageProducer(string bootstrapServers, string topic)
        {
            _bootstrapServers = bootstrapServers;
            _topic = topic;
        }

        public async Task<string> PublishAsync(ExternalMessage message)
        {
            var config = new Dictionary<string, object>
            {
                { "bootstrap.servers", _bootstrapServers }
            };
            var payload = JsonConvert.SerializeObject(message);

            using (var producer = new Producer<string, string>(config, new StringSerializer(Encoding.UTF8), new StringSerializer(Encoding.UTF8)))
            {
                await producer.ProduceAsync(_topic, message.MessageId, payload).ConfigureAwait(false);
                producer.Flush(10000);
            }

            return _topic;
        }
    }
}
