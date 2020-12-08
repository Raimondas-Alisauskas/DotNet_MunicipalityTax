namespace MunicipalityTax.WebApi.Configurations
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using MunicipalityTax.Persistence.DbContexts;

    public static class DatabaseConfig
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "***** Connection string not found");
            }

            services.AddDbContext<MtxDbContext>(
                options =>
                {
                    options.UseSqlServer(connectionString);
                }, ServiceLifetime.Transient);

            return services;
        }

        public static void UpdateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<MtxDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
