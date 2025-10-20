using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Serilog;


namespace eShop.Basket.Infrastructure.EventBus;

public class RabbitMqEventBus : IEventBus
{
    private readonly ConnectionFactory _factory;

    public RabbitMqEventBus(string hostName = "localhost", string user = "guest", string pass = "guest")
    {
        // Kørsel fra Docker → hostName = "rabbitmq"
        // Kørsel fra Visual Studio → hostName = "localhost"
        hostName ??= Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";

        _factory = new ConnectionFactory
        {
            HostName = hostName,
            Port = 5672,
            UserName = user,
            Password = pass
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

                channel.QueueDeclare(queue: eventName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "",
                                     routingKey: eventName,
                                     basicProperties: null,
                                     body: body);

                Log.Information($"[Publish] Event '{eventName}' sent (attempt {attempt})");
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



    // --- Modtager besked ---
    public void Subscribe<T>(string eventName, Action<T> handler)
    {
        var connection = _factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: eventName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var message = JsonSerializer.Deserialize<T>(json);
            if (message != null)
                handler(message);
        };

        channel.BasicConsume(queue: eventName, autoAck: true, consumer: consumer);
    }
}
