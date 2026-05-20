using Microsoft.AspNetCore.Mvc;
using TierLab.Application.UseCases.Tierlists;

namespace TierLab.Api.Controllers;

public class TierlistsController : BaseController
{
    private readonly ITierlistQueries _queries;

    public TierlistsController(ITierlistQueries queries)
    {
        _queries = queries;
    }

    [HttpGet("genero/{generoId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ListByGenero(
        long generoId,
        [FromQuery] int limit = 20,
        CancellationToken ct = default)
    {
        var result = await _queries.ListByGeneroAsync(generoId, limit, ct);
        return Ok(result.Data);
    }

    [HttpGet("busca")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromQuery] string q = "",
        CancellationToken ct = default)
    {
        var result = await _queries.SearchAsync(q, ct);
        return Ok(result.Data);
    }
}
