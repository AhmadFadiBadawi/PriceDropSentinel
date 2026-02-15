namespace PriceDropSentinel.Contracts;

public record PriceDroppedMessage(
    Guid AlertId,
    string ProductUrl,
    decimal CurrentPrice,
    decimal TargetPrice,
    string UserEmail
);

//This is a shared contract between services.
//Both the API and Worker reference this contract so they agree on message structure.

