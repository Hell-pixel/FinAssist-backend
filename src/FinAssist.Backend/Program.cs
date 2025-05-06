using System.Reflection;
using FinAssist.Backend.Filters;
using FinAssist.Backend.Middlewares;
using FinAssist.Infrastructure;
using FinAssist.Infrastructure.Configuration;
using FinAssist.Infrastructure.Extensions;
using FluentMigrator.Runner;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Newtonsoft.Json.Converters;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.Commons;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    var vaultAddress = Environment.GetEnvironmentVariable("VAULT_ADDRESS");
    var vaultToken = Environment.GetEnvironmentVariable("VAULT_TOKEN");

    if (string.IsNullOrEmpty(vaultAddress) || string.IsNullOrEmpty(vaultToken))
    {
        throw new InvalidOperationException("Vault configuration is missing for production environment.");
    }

    await builder.Configuration.LoadSecretsFromVault(vaultAddress, vaultToken);
}

AppConfiguration.Init(builder.Configuration);

var services = builder.Services;

services.ConfigureDb(builder.Configuration);
services.ConfigureDependencyContainer();
services.ConfigureAuthentication(builder.Configuration);
services.AddControllers(options =>
{
    // options.Filters.Add(typeof(LoggingActionFilter));
    options.Filters.Add<GlobalExceptionFilter>();
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new IsoDateTimeConverter()
    {
        DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ"
    });

    options.SerializerSettings.Converters.Add(new StringEnumConverter { AllowIntegerValues = false });
});
services.ConfigureHealthChecks();
services.ConfigureSwagger(Assembly.GetExecutingAssembly().GetName().Name!);
services.AddAutoMapper(Assembly.GetExecutingAssembly());

services.AddFluentValidationAutoValidation(options => { options.DisableDataAnnotationsValidation = true; });

services.AddValidatorsFromAssemblyContaining<ReflectionMarker>();
services.AddFluentValidationRulesToSwagger();

services.AddCors(o =>
{
    o.AddPolicy("dev", builder =>
    {
        builder
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("dev");
}

using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

app.UseMiddleware<RealIpMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/api/v1/health");
app.Run();
