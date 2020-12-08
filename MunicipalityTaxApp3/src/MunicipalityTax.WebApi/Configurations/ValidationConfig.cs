namespace MunicipalityTax.WebApi.Configurations
{
    using FluentValidation.AspNetCore;
    using Microsoft.Extensions.DependencyInjection;
    using MunicipalityTax.Services.Validators;

    public static class ValidationConfig
    {
        public static IMvcBuilder AddValidation(this IMvcBuilder services)
        {
            services
               .AddFluentValidation(x =>
               x.RegisterValidatorsFromAssemblyContaining<TaxScheduleCreateDtoEnumerableValidator>());

            return services;
        }
    }
}