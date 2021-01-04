namespace MunicipalityTax.WebApi.Configurations
{
    using Microsoft.Extensions.DependencyInjection;
    using MunicipalityTax.Persistence.Repositories;
    using MunicipalityTax.Services.Services;

    public static class DependecyInjectionConf
    {
        public static IServiceCollection AddDIConfiguration(this IServiceCollection services)
        {
            services.AddTransient(typeof(IBaseService<,,>), typeof(BaseService<,,>));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<ITaxScheduleService, TaxScheduleService>();
            services.AddTransient<IMunicipalityService, MunicipalityService>();
            services.AddTransient<ITaxRatesService, TaxRatesService>();
            services.AddTransient<IFileService, FileService>();

            return services;
        }
    }
}
