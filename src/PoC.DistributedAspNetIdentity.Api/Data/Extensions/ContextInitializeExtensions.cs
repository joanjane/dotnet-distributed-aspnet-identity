using Microsoft.EntityFrameworkCore;

namespace PoC.DistributedAspNetIdentity.Api.Data.Extensions
{
    public static class ContextInitializeExtensions
    {
        public static async Task InitializeDb<TDbContext>(this IServiceProvider services, IConfiguration configuration, string configurationSection) 
            where TDbContext : DbContext
        {
            if (configuration.GetSection(configurationSection).GetValue("Migrate", false))
            {
                using var scope = services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                logger.LogInformation($"Running migrations on {typeof(TDbContext).Name} context...");
                await dbContext.Database.MigrateAsync();
                logger.LogInformation($"Finished migrations on {typeof(TDbContext).Name} context...");
            }
        }
    }
}
