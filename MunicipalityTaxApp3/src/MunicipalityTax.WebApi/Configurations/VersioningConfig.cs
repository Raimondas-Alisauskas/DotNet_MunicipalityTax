namespace MunicipalityTax.WebApi.Configurations
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    public static class VersioningConfig
    {
        public static IServiceCollection AddApiVersioningWithConfig(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(Constants.Swagger.Version, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
            });
            return services;
        }
    }
}
