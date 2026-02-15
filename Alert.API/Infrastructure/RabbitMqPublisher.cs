using System.Text;
using System.Text.Json;
using PriceDropSentinel.Contracts;
using RabbitMQ.Client;

namespace Alert.API.Infrastructure;

public class RabbitMqPublisher
{
    private readonly string _hostname = "sentinel-broker";

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

//This class publishes messages to RabbitMQ.
/*
Connects to RabbitMQ (localhost)
Opens a channel
Ensures the price_drop_alerts queue exists
Serializes the message to JSON
Publishes it to the queue using the default exchange
*/
