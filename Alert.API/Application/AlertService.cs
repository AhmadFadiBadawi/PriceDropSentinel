namespace Alert.API.Application;

public class AlertService
{
    public bool ShouldNotify(decimal currentPrice, decimal targetPrice)
    {
        return currentPrice < targetPrice;
    }
}

//Implements the business logic to determine when a price alert should trigger.