using Microsoft.AspNetCore.Mvc;

namespace TierLab.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
}
