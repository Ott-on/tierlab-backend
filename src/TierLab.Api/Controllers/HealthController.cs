using Microsoft.AspNetCore.Mvc;

namespace TierLab.Api.Controllers;

/// <summary>
/// Health check endpoint for monitoring and load balancers.
/// </summary>
public class HealthController : BaseController
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Service = "TierLab API"
        });
    }
}
