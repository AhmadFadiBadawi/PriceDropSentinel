namespace Alert.API.Domain;

public class PriceAlert
{
    public Guid Id { get; set; }
    public string ProductUrl { get; set; } = string.Empty;
    public decimal TargetPrice { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public bool IsTriggered { get; set; } = false;
}