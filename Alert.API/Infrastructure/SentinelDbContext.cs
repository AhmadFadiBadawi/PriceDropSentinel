using Microsoft.EntityFrameworkCore;
using Alert.API.Domain;

namespace Alert.API.Infrastructure;

public class SentinelDbContext : DbContext
{
    public SentinelDbContext(DbContextOptions<SentinelDbContext> options) : base(options) { }
    public DbSet<PriceAlert> PriceAlerts => Set<PriceAlert>();
}

//Configures EF Core to map domain entities to the PostgreSQL database.