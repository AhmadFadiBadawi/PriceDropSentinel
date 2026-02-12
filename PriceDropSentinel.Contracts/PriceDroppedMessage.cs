namespace PriceDropSentinel.Contracts;

public record PriceDroppedMessage(
    Guid AlertId,
    string ProductUrl,
    decimal CurrentPrice,
    decimal TargetPrice,
    string UserEmail
);