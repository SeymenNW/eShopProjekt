using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Api.Services
{
    public class RabbitMqListenerService : BackgroundService
    {
        private readonly RabbitMQ.Client.IConnection _connection;  // Fully qualify IConnection
        private readonly RabbitMQ.Client.IModel _channel;          // Fully qualify IModel
        private readonly string _queueName;

        public RabbitMqListenerService(IConfiguration configuration)
        {
            // Create a connection factory to connect to RabbitMQ server
            var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = configuration["RabbitMQ:HostName"] };
            _connection = factory.CreateConnection();  // Create connection to RabbitMQ
            _channel = _connection.CreateModel();      // Create a channel to interact with RabbitMQ
            _queueName = configuration["RabbitMQ:QueueName"];

            // Declare a queue to ensure it's created if not already present
            _channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Create a consumer to listen to messages from RabbitMQ
            var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(_channel);

            // Define the logic when a message is received from the queue
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Process the received message (e.g., parse order ID)
                int orderId = int.Parse(message);
                Console.WriteLine($"Received Order ID: {orderId}");

                // Call methods or logic to handle the order processing here
            };

            // Start consuming messages from the queue
            _channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);

            // Keep the service running indefinitely
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override void Dispose()
        {
            base.Dispose();
            // Dispose of the channel and connection when done
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
