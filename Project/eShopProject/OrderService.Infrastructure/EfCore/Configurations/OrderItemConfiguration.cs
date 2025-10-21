using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using OrderService.Infrastructure.EfCore;

namespace OrderService.Infrastructure
{
    public class OrderDbContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
    {
        public OrderDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>();

            // Read configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Base path for appsettings.json
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Add json file
                .Build();

            // Use the connection string from appsettings
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("orders"));

            return new OrderDbContext(optionsBuilder.Options); // Create the DbContext with the connection string
        }
    }
}
