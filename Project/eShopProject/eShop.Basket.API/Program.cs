using eShop.Basket.Infrastructure.Data;
using eShop.Basket.Infrastructure.EventBus;
using eShop.Basket.Domain.Events; // <-- flyttet op (vigtigt)
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using eShop.Basket.Application.Interfaces;
using eShop.Basket.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// --- Load configuration ---
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// --- Serilog setup ---
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/basket-api-.log", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// --- Swagger & Controllers ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBasketService, BasketService>();

// --- JWT Authentication (deaktiveret midlertidigt) ---
var jwtKey = builder.Configuration["Jwt:Key"] ?? "SuperSecretJwtKeyForBasket123!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "eShopGateway";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// --- EF Core (PostgreSQL) ---
builder.Services.AddDbContext<BasketDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BasketDb")));

// --- EventBus (RabbitMQ) ---
builder.Services.AddSingleton<IEventBus>(sp =>
{
    var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
    var user = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
    var pass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "guest";
    return new RabbitMqEventBus(host, user, pass);
});


// --- Health checks ---
builder.Services.AddHealthChecks();

var app = builder.Build();

// --- Swagger & middleware ---
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

// --- Test subscriber (kan fjernes senere) ---
var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<BasketCheckedOutIntegrationEvent>("basket.checkedout", evt =>
{
    Console.WriteLine($"[Subscriber] Basket {evt.BasketId} checked out by {evt.CustomerId} with total {evt.Total}");
});

app.Run();

public partial class Program { }
