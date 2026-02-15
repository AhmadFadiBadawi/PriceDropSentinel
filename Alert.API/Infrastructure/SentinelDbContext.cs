using Microsoft.EntityFrameworkCore;
using Alert.API.Domain;

namespace Alert.API.Infrastructure;

public class SentinelDbContext : DbContext //EF Core's DbContext manages database connections and tracks changes to objects
{ 
    public SentinelDbContext(DbContextOptions<SentinelDbContext> options) : base(options) { } //Takes configuration options
    public DbSet<PriceAlert> PriceAlerts => Set<PriceAlert>(); //Defines a table in the database
}

