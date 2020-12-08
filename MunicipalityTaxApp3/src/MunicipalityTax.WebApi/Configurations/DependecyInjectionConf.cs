namespace MunicipalityTax.WebApi.Configurations
{
    using Microsoft.Extensions.DependencyInjection;
    using MunicipalityTax.Persistence.DbContexts;
    using MunicipalityTax.Persistence.Repositories;
    using MunicipalityTax.Services.Services;

    public static class DependecyInjectionConf
    {
        public static IServiceCollection AddDIConfiguration(this IServiceCollection services)
        {
            services
                .AddTransient(typeof(IBaseService<,,>), typeof(BaseService<,,>))
                .AddTransient(typeof(IRepository<>), typeof(Repository<>))
                .AddTransient<MtxDbContext>()
                .AddTransient<ITaxScheduleService, TaxScheduleService>()
                .AddTransient<IMunicipalityService, MunicipalityService>()
                .AddTransient<ITaxRatesService, TaxRatesService>();

            return services;
        }
    }
}
