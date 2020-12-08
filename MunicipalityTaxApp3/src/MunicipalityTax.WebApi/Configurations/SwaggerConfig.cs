namespace MunicipalityTax.WebApi.Configurations
{
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public static class SwaggerConfig
    {
        private static readonly string ApiVersion = $"v{Constants.Swagger.Version}";
        private static readonly string ApiName = Constants.Swagger.ApiName;
        private static readonly string SwaggerEndPoint = Constants.Swagger.EndPoint;

        public static IServiceCollection AddSwaggerWithConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(
                    ApiVersion,
                    new OpenApiInfo
                    {
                        Title = ApiName,
                        Version = ApiVersion,
                    });
                config.OperationFilter<RemoveVersionFromParameter>();
                config.DocumentFilter<ReplaceVersionWithExactValueInPath>();
            });
            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }

        public static IApplicationBuilder UseSwaggerWithOptions(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(SwaggerEndPoint, ApiName + " " + ApiVersion);

                // options.RoutePrefix = string.Empty;
            });

            app.Use(async (httpContext, next) =>
            {
                if (string.IsNullOrEmpty(httpContext.Request.Path) ||
                    httpContext.Request.Path == "/" ||
                    httpContext.Request.Path == "/api")
                {
                    httpContext.Response.Redirect(httpContext.Request.PathBase + "/swagger");
                    return;
                }

                await next();
            });

            return app;
        }

        private class RemoveVersionFromParameter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var versionParameter = operation.Parameters.Single(p => p.Name == "version");
                operation.Parameters.Remove(versionParameter);
            }
        }

        private class ReplaceVersionWithExactValueInPath : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                var oap = new OpenApiPaths();
                foreach (var p in swaggerDoc.Paths)
                {
                    oap.Add(
                        p.Key.Replace("v{version}", swaggerDoc.Info.Version),
                        p.Value);
                }

                swaggerDoc.Paths = oap;
            }
        }
    }
}
