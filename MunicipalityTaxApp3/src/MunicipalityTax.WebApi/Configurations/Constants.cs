namespace MunicipalityTax.WebApi.Configurations
{
    public static class Constants
    {
        public static class Swagger
        {
            public static int Version => 1;

            public static string EndPoint => $"/swagger/v{Version}/swagger.json";

            public static string ApiName => "Municipality Tax API";
        }

        public static class Health// todo: health https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-2.2
        {
            public static string EndPoint => "/health";
        }
    }
}
