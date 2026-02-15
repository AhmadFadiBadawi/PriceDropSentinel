using Microsoft.AspNetCore.Mvc;
using Alert.API.Domain;
using Alert.API.Infrastructure;
using PriceDropSentinel.Contracts;

namespace Alert.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlertsController : ControllerBase
{
    private readonly IPriceAlertRepository _repository;
    private readonly RabbitMqPublisher _publisher;

    public AlertsController(IPriceAlertRepository repository, RabbitMqPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAlert([FromBody] PriceAlert alert)
    {
        // 1. Save to PostgreSQL (Data Persistence) [cite: 10, 17]
        await _repository.AddAsync(alert);

        // 2. Wrap in a Contract Message [cite: 21]
        var message = new PriceDroppedMessage(
            alert.Id, alert.ProductUrl, 100.00m, alert.TargetPrice, alert.UserEmail);

        // 3. Publish to RabbitMQ (Async Messaging) 
        await _publisher.PublishPriceDropAsync(message);

        return Ok(new { Message = "Alert created and queued!" });
    }
}