namespace Alert.API.Domain;

public interface IPriceAlertRepository
{
    Task AddAsync(PriceAlert alert);
    Task<IEnumerable<PriceAlert>> GetActiveAlertsAsync();
    Task UpdateAsync(PriceAlert alert);
}

//Declares the interface for storing and retrieving price alerts.