using System.Text;
using System.Text.Json;
using PriceDropSentinel.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Notification.Service;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger) => _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "price_drop_alerts", 
            durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = JsonSerializer.Deserialize<PriceDroppedMessage>(Encoding.UTF8.GetString(body));

            if (message != null)
            {
                _logger.LogInformation("Price Drop Detected! Sending email to {Email} for {Url}", 
                    message.UserEmail, message.ProductUrl);
            }
        };

        await channel.BasicConsumeAsync(queue: "price_drop_alerts", autoAck: true, consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}

//This is a background service that listens for messages.
/*
Listens to the same queue
Receives the message
Deserializes it
Processes it (e.g., sends email)
*/