using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sastral.DbGenerator.Application.Abstractions;
using Sastral.DbGenerator.Infrastructure.Persistence.Contexts;

namespace Sastral.DbGenerator.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<SastralDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Register UnitOfWork
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<SastralDbContext>());

        return services;
    }
}
