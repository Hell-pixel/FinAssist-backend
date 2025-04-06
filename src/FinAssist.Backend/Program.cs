using System.Reflection;
using FinAssist.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.ConfigureControllers();
services.ConfigureHealthChecks();
services.ConfigureSwagger(Assembly.GetExecutingAssembly().GetName().Name!);
services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();