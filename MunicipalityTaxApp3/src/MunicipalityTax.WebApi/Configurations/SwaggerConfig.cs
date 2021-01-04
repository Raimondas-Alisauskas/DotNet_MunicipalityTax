namespace MunicipalityTax.WebApi.Configurations
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    ApiVersion,
                    new OpenApiInfo
                    {
                        Title = ApiName,
                        Version = ApiVersion,
                        Description = "Example of ASP.NET Core Web API which manages taxes applied in different municipalities",
                    });
                c.OperationFilter<RemoveVersionFromParameter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
            });
            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }

        public static IApplicationBuilder UseSwaggerWithOptions(this IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(SwaggerEndPoint, ApiName + " " + ApiVersion);

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
