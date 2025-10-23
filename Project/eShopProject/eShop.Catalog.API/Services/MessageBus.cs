using eShop.Catalog.API.Dto;
using RabbitMQ.Client;

namespace eShop.Catalog.API.Services
{
    public class MessageBus
    {
        private readonly IConnectionFactory _connectionFactory;

        public MessageBus(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task PublishCatalogItem(ItemDto catalogItem)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: "catalog.items",
                type: ExchangeType.Fanout
                );

        }



    }
}
