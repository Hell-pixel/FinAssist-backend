using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FinAssist.Backend.Controllers;

/// <summary>
/// Controller for checking the application health.
/// </summary>
[Route("api/v1/health")]
public class HealthCheckController : ControllerBase
{
    private readonly HealthCheckService _healthCheckService;

    public HealthCheckController(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    /// <summary>
    /// Gets the application's health report.
    /// </summary>
    /// <returns>The application's health report.</returns>
    /// <response code="200">The application's health report.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<HealthReport> Get()
    {
        return await _healthCheckService.CheckHealthAsync();
    }
}