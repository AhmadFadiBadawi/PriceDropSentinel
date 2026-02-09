namespace Alert.API.Application;

public class AlertService
{
    public bool ShouldNotify(decimal currentPrice, decimal targetPrice)
    {
        return currentPrice < targetPrice; // 
    }
}