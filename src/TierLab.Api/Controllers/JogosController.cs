using Microsoft.AspNetCore.Mvc;
using TierLab.Application.UseCases.Jogos;

namespace TierLab.Api.Controllers;

public sealed class JogosController : BaseController
{
    private readonly IJogoQueries _queries;

    public JogosController(IJogoQueries queries)
    {
        _queries = queries;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var result = await _queries.GetAllAsync(ct);
        return Ok(result.Data);
    }

    [HttpGet("busca")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromQuery] string q = "",
        CancellationToken ct = default)
    {
        var result = await _queries.SearchByTituloAsync(q, ct);
        return Ok(result.Data);
    }
}
