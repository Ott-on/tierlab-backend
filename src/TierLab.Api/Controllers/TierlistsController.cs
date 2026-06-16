using Microsoft.AspNetCore.Mvc;
using TierLab.Application.UseCases.Tierlists;
using TierLab.Application.UseCases.Tierlists.Requests;

namespace TierLab.Api.Controllers;

public class TierlistsController : BaseController
{
    private readonly ITierlistQueries _queries;
    private readonly ITierlistService _service;

    public TierlistsController(
        ITierlistQueries queries,
        ITierlistService service)
    {
        _queries = queries;
        _service = service;
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

    [HttpGet("usuario/{usuarioId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ListByUsuario(
        Guid usuarioId,
        [FromQuery] int limit = 20,
        CancellationToken ct = default)
    {
        var result = await _queries.ListByUsuarioAsync(usuarioId, limit, ct);
        return Ok(result.Data);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List(
        [FromQuery] int limit = 20,
        [FromQuery] string? cursor = null,
        CancellationToken ct = default)
    {
        var request = new TierlistsCursorRequest(limit, cursor);
        var result = await _queries.ListByCursorAsync(request, ct);

        if (!result.Success)
            return BadRequest(result.Errors);

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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTierlistRequest request,
        CancellationToken ct = default)
    {
        if (request.UsuarioId == Guid.Empty)
            return BadRequest("UsuarioId é obrigatório.");

        var result = await _service.CreateAsync(request.UsuarioId, request, ct);
        if (!result.Success)
            return BadRequest(result.Errors);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPost("{tierlistId:long}/jogos")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddJogo(
        long tierlistId,
        [FromBody] AddTierlistJogoRequest request,
        CancellationToken ct = default)
    {
        var result = await _service.AddJogoAsync(tierlistId, request, ct);
        if (!result.Success)
            return BadRequest(result.Errors);

        return NoContent();
    }

    [HttpDelete("{tierlistId:long}/jogos/{jogoId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveJogo(
        long tierlistId,
        long jogoId,
        CancellationToken ct = default)
    {
        var result = await _service.RemoveJogoAsync(tierlistId, jogoId, ct);
        if (!result.Success)
            return NotFound(result.Errors);

        return NoContent();
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct = default)
    {
        var result = await _queries.GetByIdAsync(id, ct);
        if (!result.Success || result.Data is null)
            return NotFound(result.Errors);

        return Ok(result.Data);
    }

    [HttpGet("{tierlistId:long}/jogos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListJogos(long tierlistId, [FromQuery] int limit = 20, [FromQuery] string? cursor = null, CancellationToken ct = default)
    {
        var result = await _queries.ListJogosByCursorAsync(tierlistId, limit, cursor, ct);
        if (!result.Success)
        {
            // treat not found as 404 when repository indicates missing tierlist
            if (result.Errors?.Any() == true && result.Errors.Contains("Tierlist não encontrada."))
                return NotFound(result.Errors);

            return BadRequest(result.Errors);
        }

        return Ok(result.Data);
    }

}
