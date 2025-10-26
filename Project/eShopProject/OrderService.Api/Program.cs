using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Abstractions;
using OrderService.Infrastructure.EfCore;
using OrderService.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// DbContext → PostgreSQL
builder.Services.AddDbContext<OrderDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("orders"))
// Uncomment if you want snake_case tables/columns automatically:
//.UseSnakeCaseNamingConvention()
);
builder.Services.AddHostedService<OrderService.Api.Services.RabbitMqListenerService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
