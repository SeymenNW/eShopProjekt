using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace eShop.Basket.Infrastructure.Messaging;

public class RabbitMqClient
{
    private readonly ConnectionFactory _factory;

    public RabbitMqClient()
    {
        // Hvis du kører i Docker, findes miljøvariablen RABBITMQ_HOST=rabbitmq
        // Hvis du kører i Visual Studio, bruges "localhost"
        var hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";

        _factory = new ConnectionFactory
        {
            HostName = hostName,
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };
    }


    public void Publish<T>(string queueName, T message)
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(
            exchange: "",
            routingKey: queueName,
            basicProperties: null,
            body: body
        );

        Console.WriteLine($"[RabbitMQ] Published message to '{queueName}'");
    }
}
