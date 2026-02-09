using Alert.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace Alert.API.Infrastructure;

public class PriceAlertRepository : IPriceAlertRepository
{
    private readonly SentinelDbContext _context;

    public PriceAlertRepository(SentinelDbContext context) => _context = context;

    public async Task AddAsync(PriceAlert alert)
    {
        await _context.PriceAlerts.AddAsync(alert);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PriceAlert>> GetActiveAlertsAsync()
    {
        return await _context.PriceAlerts.Where(a => !a.IsTriggered).ToListAsync();
    }

    public async Task UpdateAsync(PriceAlert alert)
    {
        _context.PriceAlerts.Update(alert);
        await _context.SaveChangesAsync();
    }
}