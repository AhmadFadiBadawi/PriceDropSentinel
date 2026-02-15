using Alert.API.Domain;
using Alert.API.Infrastructure;
using Alert.API.Application;
using Microsoft.EntityFrameworkCore;
using Npgsql; // Added for exception handling

var builder = WebApplication.CreateBuilder(args);

// 1. Database Context
builder.Services.AddDbContext<SentinelDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Dependency Injection Registrations
builder.Services.AddScoped<IPriceAlertRepository, PriceAlertRepository>();
builder.Services.AddScoped<AlertService>();
builder.Services.AddSingleton<RabbitMqPublisher>(); 

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// 3. Development Middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// 4. Robust Database Initialization with Retry Logic
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var db = services.GetRequiredService<SentinelDbContext>();

    int retries = 5;
    while (retries > 0)
    {
        try
        {
            logger.LogInformation("Attempting to connect to database...");
            db.Database.EnsureCreated();
            logger.LogInformation("Database connection successful!");
            break; 
        }
        catch (Exception ex)
        {
            retries--;
            logger.LogWarning($"Database not ready yet. Retrying in 5s... ({retries} attempts left)");
            Thread.Sleep(5000); // Wait 5 seconds before trying again
            
            if (retries == 0)
            {
                logger.LogCritical("Could not connect to database. Exiting.");
                throw; 
            }
        }
    }
}

// 5. Middleware Pipeline
app.UseAuthorization();
app.MapControllers();

app.Run();