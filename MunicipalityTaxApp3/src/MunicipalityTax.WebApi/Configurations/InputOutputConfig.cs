namespace MunicipalityTax.WebApi.Configurations
{
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    public static class InputOutputConfig
    {
        public static IMvcBuilder AddIOFormatter(this IMvcBuilder services)
        {
            services.AddNewtonsoftJson(options =>
             {
                 options.SerializerSettings.Converters.Add(new StringEnumConverter());
                 options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
             });
            services.AddXmlDataContractSerializerFormatters();

            return services;
        }
    }
}
