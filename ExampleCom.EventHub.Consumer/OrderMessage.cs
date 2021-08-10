using Confluent.Kafka;
using ExampleCom.EventHub.POCOs;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ExampleCom.EventHub.Consumer
{
    public class OrderMessage
    {
        public OrderMessage()
        {

        }
        public async Task<Order> Read(string bootStrapServers, string userName, string password, string topic)
        {
            try
            {
                var config = new ConsumerConfig
                {
                    GroupId = "test-consumer-group",
                    BootstrapServers = bootStrapServers,
                    SecurityProtocol = SecurityProtocol.SaslSsl,
                    SaslMechanism = SaslMechanism.ScramSha256,
                    SaslUsername = userName,
                    SaslPassword = password,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                };
                using var c = new ConsumerBuilder<Ignore, string>(config).Build();
                c.Subscribe(topic);
                
                CancellationTokenSource cts = new CancellationTokenSource();
                var cr = await Task.FromResult(c.Consume(cts.Token));
                var json = cr.Message.Value;
                var order = JsonSerializer.Deserialize<Order>(json);
                Console.WriteLine($"Consumed message \n'{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                return order;
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Error occurred: {e.Error.Reason}");
                return null;
            }

        }
    }
}
