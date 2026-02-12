using System.Text;
using System.Text.Json;
using PriceDropSentinel.Contracts;
using RabbitMQ.Client;

namespace Alert.API.Infrastructure;

public class RabbitMqPublisher
{
    private readonly string _hostname = "localhost";

    public async Task PublishPriceDropAsync(PriceDroppedMessage message)
    {
        var factory = new ConnectionFactory { HostName = _hostname };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "price_drop_alerts", 
            durable: false, exclusive: false, autoDelete: false, arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(exchange: string.Empty, 
            routingKey: "price_drop_alerts", body: body);
    }
}