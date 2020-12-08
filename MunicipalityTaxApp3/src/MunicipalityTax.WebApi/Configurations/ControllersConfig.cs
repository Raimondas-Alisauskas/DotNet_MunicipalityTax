namespace MunicipalityTax.WebApi.Configurations
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ControllersConfig
    {
        public static IMvcBuilder AddControllersWithConfig(this IServiceCollection services)
        {
            return services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            });
        }
    }
}
