using Microsoft.AspNetCore.Mvc;

namespace TierLab.Api.Controllers;

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
