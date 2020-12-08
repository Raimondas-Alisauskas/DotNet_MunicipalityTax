namespace MunicipalityTax.WebApi
{
    using AutoMapper;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using MunicipalityTax.Services.Mappers;
    using MunicipalityTax.WebApi.Configurations;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = this.Configuration.GetSection("DatabaseConnectionString").Value;
            services
                .AddControllersWithConfig()
                .AddValidation()
                .AddIOFormatter();
            services.AddDIConfiguration()
                .AddDatabaseContext(connectionString)
                .AddAutoMapper(typeof(AutomapProfile))
                .AddSwaggerWithConfig()
                .AddApiVersioningWithConfig();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<ExceptionHandler>();
            }

            app.UseHttpsRedirection();

            app.UseSwaggerWithOptions();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UpdateDatabase();

            logger.LogInformation("***** Server configuration is completed");
        }
    }
}
