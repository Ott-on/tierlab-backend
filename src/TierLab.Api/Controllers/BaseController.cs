using Microsoft.AspNetCore.Mvc;

namespace TierLab.Api.Controllers;

/// <summary>
/// Base controller providing shared functionality for all API controllers.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
}
