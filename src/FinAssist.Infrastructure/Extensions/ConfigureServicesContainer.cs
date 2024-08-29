using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;

namespace FinAssist.Infrastructure.Extensions;

public static class ConfigureServicesContainer
{
    public static void ConfigureSwagger(this IServiceCollection services, string assemblyName)
    {
        services.AddSwaggerGen(c =>
        {
            c.MapType<HealthReport>(() => new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["entries"] = new() { Type = "object" },
                    ["status"] = new()
                    {
                        Type = "string",
                        Enum = new IOpenApiAny[]
                        {
                            new OpenApiString(HealthStatus.Healthy.ToString()),
                            new OpenApiString(HealthStatus.Degraded.ToString()),
                            new OpenApiString(HealthStatus.Unhealthy.ToString())
                        },
                    },
                    ["totalDuration"] = new()
                    {
                        Type = "string",
                        Format = "duration",
                        Example = new OpenApiString("00:00:00.0000110")
                    }
                }
            });

            c.SwaggerDoc("v1", new OpenApiInfo { Title = "FinAssist", Version = "v1" });

            var xmlFile = $"{assemblyName}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
    }

    public static void ConfigureHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks();
    }

    public static void ConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter { AllowIntegerValues = false });
            });
    }
}