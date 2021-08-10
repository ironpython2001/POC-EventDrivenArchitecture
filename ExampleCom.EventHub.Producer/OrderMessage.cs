using Confluent.Kafka;
using ExampleCom.EventHub.POCOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExampleCom.EventHub.Producer
{
    public class OrderMessage
    {
        public OrderMessage()
        {

        }
        public async Task<bool> Create(string bootStrapServers,string userName,string password,string topic,Order request)
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = bootStrapServers,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.ScramSha256,
                SaslUsername = userName,
                SaslPassword = password,
            };

            var result = false;

            using var p = new ProducerBuilder<Null, string>(config).Build();
            try
            {
                var json = JsonSerializer.Serialize(request);
                var dr = await p.ProduceAsync($"{topic}", new Message<Null, string>
                {
                    Value = json
                });
                Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                result = true;
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                result = false;
            }
            return result;

        }
    }
}
