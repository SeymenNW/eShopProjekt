using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Serilog;

namespace eShop.BuildingBlocks.EventBus
{
    public class RabbitMqEventBus : IEventBus
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqEventBus(string hostName = "localhost", string user = "guest", string pass = "guest")
        {
            // Docker → "rabbitmq", lokal udvikling → "localhost"
            hostName ??= Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";

            _factory = new ConnectionFactory
            {
                HostName = hostName,
                Port = 5672,
                UserName = user,
                Password = pass,
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
        }

        // --- Sender besked ---
        public void Publish<T>(string eventName, T message)
        {
            var maxRetries = 3;
            var retryDelay = TimeSpan.FromSeconds(5);

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    using var connection = _factory.CreateConnection();
                    using var channel = connection.CreateModel();

                    // Opret exchange og queue (durable)
                    channel.ExchangeDeclare(exchange: "amq.topic", type: "topic", durable: true);
                    channel.QueueDeclare(queue: eventName, durable: true, exclusive: false, autoDelete: false);
                    channel.QueueBind(queue: eventName, exchange: "amq.topic", routingKey: eventName);

                    // Gør beskeden persistent
                    var props = channel.CreateBasicProperties();
                    props.Persistent = true;

                    var json = JsonSerializer.Serialize(message);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "amq.topic",
                                         routingKey: eventName,
                                         basicProperties: props,
                                         body: body);

                    Log.Information($"[Publish] Event '{eventName}' sent successfully");
                    break;
                }
                catch (Exception ex)
                {
                    Log.Warning($"[Retry {attempt}] Failed to publish '{eventName}': {ex.Message}");
                    if (attempt == maxRetries)
                    {
                        Log.Error($"[Error] Event '{eventName}' failed after {maxRetries} attempts");
                        throw;
                    }

                    Thread.Sleep(retryDelay);
                }
            }
        }

        // --- Modtager besked (Subscriber) ---
        public void Subscribe<T>(string eventName, Action<T> handler)
        {
            //var connection = _factory.CreateConnection();
            //var channel = connection.CreateModel();

            //channel.ExchangeDeclare(exchange: "amq.topic", type: "topic", durable: true);
            //channel.QueueDeclare(queue: eventName, durable: true, exclusive: false, autoDelete: false);
            //channel.QueueBind(queue: eventName, exchange: "amq.topic", routingKey: eventName);

            //var consumer = new AsyncEventingBasicConsumer(channel);
            //consumer.Received += async (model, ea) =>
            //{
            //    try
            //    {
            //        var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            //        var message = JsonSerializer.Deserialize<T>(json);
            //        if (message != null)
            //        {
            //            handler(message);
            //            Log.Information($"[Consume] Event '{eventName}' processed.");
            //        }

            //        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Error($"[Consume Error] {ex.Message}");
            //        channel.BasicNack(ea.DeliveryTag, false, true); // requeue on failure
            //    }

            //    await Task.Yield();
            //};

            //channel.BasicConsume(queue: eventName, autoAck: false, consumer: consumer);

            //Log.Information($"[Subscribe] Listening for '{eventName}' events...");
        }
    }
}
