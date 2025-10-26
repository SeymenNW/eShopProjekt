using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using OrderService.Domain.Abstractions;
using OrderService.Domain.Models;
using OrderService.Api.Contracts.Requests; 
using OrderService.Domain.Models; // For Address and ItemSnapshot

namespace OrderService.Api.Messaging
{
    
    public sealed class RabbitMqOrderConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RabbitMqOrderConsumer> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string QueueName = "order-create-queue"; 

        public RabbitMqOrderConsumer(IServiceProvider serviceProvider, ILogger<RabbitMqOrderConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            // RabbitMQ Connection Setup (Configuration should be externalized)
            var factory = new ConnectionFactory() { HostName = "localhost" /* Use configuration value */ };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: QueueName,
                                      durable: true, // Queue survives broker restarts
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);

                _logger.LogInformation("RabbitMQ connection and channel set up successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not connect to RabbitMQ.");
                // Handle connection failure, e.g., stop the service gracefully
                throw;
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation("Received message: {Message}", message);

                try
                {
                    var request = JsonSerializer.Deserialize<CreateOrderRequest>(message);
                    if (request != null)
                    {
                        // Use a separate scope for the repository to ensure it's properly disposed
                        using var scope = _serviceProvider.CreateScope();
                        var repo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                        // Process the message and save the order (same logic as in your controller)
                        await ProcessOrderMessage(repo, request, stoppingToken);

                        // Acknowledge the message to remove it from the queue
                        _channel.BasicAck(ea.DeliveryTag, multiple: false);
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Failed to deserialize RabbitMQ message.");
                    // Nack and potentially requeue for retry, or move to a dead-letter queue (DLQ)
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing order message.");
                    // Decide on requeue strategy (false = discard/DLQ, true = requeue for retry)
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
            };

            _channel.BasicConsume(queue: QueueName,
                                  autoAck: false, // Important: We manually acknowledge/nack
                                  consumer: consumer);

            return Task.CompletedTask;
        }

        private async Task ProcessOrderMessage(IOrderRepository repo, CreateOrderRequest r, CancellationToken ct)
        {
            // --- Order Creation Logic from your Controller ---

            // Create order and add items
            var order = new Order(r.BuyerId, new Address(r.ShipTo.Street, r.ShipTo.City, r.ShipTo.Zip, r.ShipTo.Country));

            foreach (var it in r.Items)
            {
                // Basic validation (more comprehensive validation should be done by the sender)
                if (it.UnitPrice <= 0 || it.Units <= 0)
                {
                    // Log and potentially throw, or skip/DLQ invalid message
                    _logger.LogWarning("Invalid order item data received for BuyerId: {BuyerId}", r.BuyerId);
                    // In a real scenario, you might want to stop processing and Nack the message
                    continue;
                }

                var snap = new ItemSnapshot(it.ItemId, it.Name, it.PictureUri);
                order.AddItem(snap, it.UnitPrice, it.Units);
            }

            // Save order to the repository
            await repo.AddAsync(order, ct);
            await repo.SaveChangesAsync(ct);

            _logger.LogInformation("Order {OrderId} for Buyer {BuyerId} successfully created from RabbitMQ message.", order.Id, order.BuyerId);

            // --- End Order Creation Logic ---
        }

        // Clean up resources on stop
        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}