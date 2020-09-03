using System.Reflection;
using BetterTravel.API.ApiConstants;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace BetterTravel.API.Extensions.ApplicationBuilder
{
    public static partial class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBetterTravelSwaggerUi(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseSwagger().UseSwaggerUI(SetupSwaggerUiOptions);

            static void SetupSwaggerUiOptions(SwaggerUIOptions options)
            {
                options.DisplayRequestDuration();
                options.DocumentTitle = typeof(Startup).Assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
                options.SwaggerEndpoint($"/swagger/{ApiVersions.V1}/swagger.json", ApiVersions.V1);
                options.RoutePrefix = "api/docs";
            }
        }
    }
}