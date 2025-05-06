using System.Text;
using FinAssist.Domain.Common;
using FinAssist.Domain.Configuration;
using FinAssist.Infrastructure.Persistence.Context;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using FinAssist.Infrastructure.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace FinAssist.Infrastructure.Extensions;

public static class ConfigureServicesContainer
{
    public static void ConfigureDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (connectionString is null)
        {
            throw new Exception("Connection string not found.");
        }

        services.AddSingleton(new DapperContext(connectionString));
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(ReflectionMarker).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());
    }

    public static void ConfigureSwagger(this IServiceCollection services, string assemblyName)
    {
        services.AddSwaggerGen(c =>
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Name = "Bearer Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Введите **_ТОЛЬКО_** свой токен JWT Bearer в текстовое поле ниже!",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                },
            };
            
            c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, new List<string>() }
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
        services.AddControllers(options => { options.Filters.Add(new ProducesAttribute("application/json")); })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter { AllowIntegerValues = false });
            });
    }

    public static void ConfigureDependencyContainer(this IServiceCollection services)
    {
        var appRepositories = typeof(ReflectionMarker).Assembly.GetTypes()
            .Where(s => s.Name.EndsWith("Repository") && s.IsInterface == false).ToList();

        foreach (var appRepository in appRepositories)
        {
            var interfaceType = appRepository.GetInterfaces().FirstOrDefault(x => x.Name.EndsWith("Repository"));
            if (interfaceType is not null)
            {
                services.Add(new ServiceDescriptor(interfaceType, appRepository, ServiceLifetime.Scoped));
            }
        }

        var appServices = typeof(ReflectionMarker).Assembly.GetTypes()
            .Where(s => s.Name.EndsWith("Service") && s.IsInterface == false).ToList();
        foreach (var appService in appServices)
        {
            var interfaceType = appService.GetInterfaces().FirstOrDefault(x => x.Name.EndsWith("Service"));

            if (interfaceType is not null)
            {
                services.Add(new ServiceDescriptor(interfaceType, appService, ServiceLifetime.Scoped));
            }
        }
    }

    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfiguration = configuration.GetSection("JWT").Get<JwtConfiguration>();

        if (jwtConfiguration is null)
        {
            throw new Exception("JWT configuration is missing");
        }

        services.AddAuthentication(Constants.AuthScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,

                    ValidateIssuer = true,
                    ValidIssuer = jwtConfiguration.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtConfiguration.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.SecurityKey))
                };
            }).AddScheme<AppAuthenticationSchemeOptions, AuthHandler>(Constants.AuthScheme, _ => { });
    }
}