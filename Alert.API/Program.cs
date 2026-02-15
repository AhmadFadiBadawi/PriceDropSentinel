using Alert.API.Domain;
using Alert.API.Infrastructure;
using Alert.API.Application;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Context
builder.Services.AddDbContext<SentinelDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Dependency Injection Registrations
builder.Services.AddScoped<IPriceAlertRepository, PriceAlertRepository>();
builder.Services.AddScoped<AlertService>();

// Changed to Singleton for better RabbitMQ connection management
builder.Services.AddSingleton<RabbitMqPublisher>(); 

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// 3. Development Middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// 4. Database Auto-Migration (Ensure tables exist on startup)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SentinelDbContext>();
    // For production, use db.Database.Migrate(); 
    // EnsureCreated() is fine for our current phase.
    db.Database.EnsureCreated();
}

// 5. Middleware Pipeline
// app.UseHttpsRedirection(); // Commented out for easier local Docker testing
app.UseAuthorization();
app.MapControllers();

app.Run();