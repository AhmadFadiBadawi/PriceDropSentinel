using Alert.API.Domain;
using Alert.API.Infrastructure;
using Alert.API.Application;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SentinelDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPriceAlertRepository, PriceAlertRepository>();
builder.Services.AddScoped<AlertService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SentinelDbContext>();
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();