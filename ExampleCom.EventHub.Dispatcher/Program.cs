using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using ExampleCom.EventHub.POCOs;
using Microsoft.VisualStudio.Threading;
using ExampleCom.EventHub.Consumer;
using System.Net.Http;
using System.Text;

namespace ExampleCom.EventHub.Dispatcher
{
    class Program
    {
        public static IConfigurationRoot configuration;
        static async Task Main(string[] args)
        {
            configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json", false)
            .Build();

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);

            var tasks = new Task[]
            {
                Task.Run(async () =>
                {
                    await PollToOrderReceivedEventsAsync();
                }),
                Task.Run(async () =>
                {
                    await PollToOrderCreatedEventsAsync();
                })
            };
            await Task.WhenAll(tasks);
        }

        private static async Task PollToOrderReceivedEventsAsync()
        {
            Console.WriteLine("PollToOrderReceivedEventsAsync");
            for (; ; )
            {
                //try
                //{
                var bootstrapServers = configuration["BootstrapServers"].ToString();
                var username = configuration["UserName"].ToString();
                var password = configuration["Password"].ToString();
                var topic = $"8rvw9okw-order-received";
                var url = "https://localhost:5004/Order/createorder"; //call createdb service

                var msg = new OrderMessage();
                var order = await msg.Read(bootstrapServers, username, password, topic);
                var orderJson = JsonSerializer.Serialize(order);
                HttpClient client = new HttpClient();
                var content = new StringContent(orderJson, Encoding.UTF8, "application/json");
                Console.WriteLine($"sending request to {url}");
                _ = client.PostAsync(url, content);
                //var config = new ConsumerConfig
                //{
                //    GroupId = "test-consumer-group",
                //    BootstrapServers = bootstrapServers,
                //    SecurityProtocol = SecurityProtocol.SaslSsl,
                //    SaslMechanism = SaslMechanism.ScramSha256,
                //    SaslUsername = username,
                //    SaslPassword = password,
                //    AutoOffsetReset = AutoOffsetReset.Earliest,
                //};
                //using var c = new ConsumerBuilder<Ignore, string>(config).Build();
                //c.Subscribe(topic);

                //CancellationTokenSource cts = new CancellationTokenSource();
                //var cr = await Task.FromResult(c.Consume(cts.Token));
                //var weatherRequestJson = cr.Message.Value;
                //var weatherRequest = JsonSerializer.Deserialize<Order>(weatherRequestJson);
                //Console.WriteLine($"PollToOrderReceivedEventsAsync Consumed message \n'{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");

                //}
                //catch (ConsumeException e)
                //{
                //    Console.WriteLine($"Error occurred: {e.Error.Reason}");
                //}
            }
        }
        private static async Task PollToOrderCreatedEventsAsync()
        {
            Console.WriteLine("PollToOrderCreatedEventsAsync");
            for (; ; )
            {
                //try
                //{
                var bootstrapServers = configuration["BootstrapServers"].ToString();
                var username = configuration["UserName"].ToString();
                var password = configuration["Password"].ToString();
                var topic = $"8rvw9okw-order-created";
                var url = "https://localhost:5005/Order/ordermail"; //call createdb service

                var msg = new OrderMessage();
                var order = await msg.Read(bootstrapServers, username, password, topic);
                var orderJson = JsonSerializer.Serialize(order);
                HttpClient client = new HttpClient();
                var content = new StringContent(orderJson, Encoding.UTF8, "application/json");
                Console.WriteLine($"sending request to {url}");
                _ = client.PostAsync(url, content);
                //        var config = new ConsumerConfig
                //        {
                //            GroupId = "test-consumer-group",
                //            BootstrapServers = bootstrapServers,
                //            SecurityProtocol = SecurityProtocol.SaslSsl,
                //            SaslMechanism = SaslMechanism.ScramSha256,
                //            SaslUsername = username,
                //            SaslPassword = password,
                //            AutoOffsetReset = AutoOffsetReset.Earliest,
                //        };
                //    using var c = new ConsumerBuilder<Ignore, string>(config).Build();
                //    c.Subscribe(topic);

                //    CancellationTokenSource cts = new CancellationTokenSource();
                //    var cr = await Task.FromResult(c.Consume(cts.Token));
                //    var weatherRequestJson = cr.Message.Value;
                //    var weatherRequest = JsonSerializer.Deserialize<Order>(weatherRequestJson);
                //    Console.WriteLine($"PollToOrderCreatedEventsAsync Consumed message \n'{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                //}
                //    catch (ConsumeException e)
                //{
                //    Console.WriteLine($"Error occurred: {e.Error.Reason}");
                //}
            }
        }


    }
}
