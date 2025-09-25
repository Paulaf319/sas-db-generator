using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sastral.DbGenerator.Infrastructure;
using Sastral.DbGenerator.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Sastral.DbGenerator.Console;

class Program
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            logger.LogInformation("Starting Sastral Database Generator...");

            var context = services.GetRequiredService<SastralDbContext>();
            
            logger.LogInformation("Ensuring database is created...");
            await context.Database.EnsureCreatedAsync();
            
            logger.LogInformation("Running pending migrations...");
            await context.Database.MigrateAsync();
            
            logger.LogInformation("Database setup completed successfully!");
            
            // Optional: Seed initial data
            await SeedInitialDataAsync(context, logger);
            
            logger.LogInformation("Sastral Database Generator completed successfully!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while setting up the database");
            throw;
        }
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddInfrastructure(context.Configuration);
            });

    static async Task SeedInitialDataAsync(SastralDbContext context, ILogger logger)
    {
        logger.LogInformation("Checking if initial data seeding is needed...");

        // Check if roles already exist (they should be seeded via configuration)
        var rolesCount = await context.Roles.CountAsync();
        if (rolesCount > 0)
        {
            logger.LogInformation("Roles already exist. Skipping initial data seeding.");
            return;
        }

        logger.LogInformation("Seeding initial data...");

        // The roles are seeded via EF configuration, but we can add a default admin user here if needed
        // This would typically be done in a separate seeding process in a real application

        await context.SaveChangesAsync();
        logger.LogInformation("Initial data seeding completed.");
    }
}
