using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TierLab.Application.UseCases.Usuarios;
using TierLab.Application.UseCases.Usuarios.Requests;
using TierLab.Application.UseCases.Usuarios.Requests;

namespace TierLab.Api.Controllers;

public sealed class UsersController : BaseController
{
    private readonly IUsuarioService _usuarioService;

    public UsersController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCurrent(
        [FromQuery] Guid usuarioId,
        [FromQuery] string? email,
        [FromQuery] string? username,
        [FromQuery] string? imageUrl,
        CancellationToken ct = default)
    {
        if (usuarioId == Guid.Empty)
            return BadRequest("UsuarioId é obrigatório.");

        var result = await _usuarioService.GetOrCreateCurrentAsync(
            usuarioId,
            email,
            username,
            imageUrl,
            ct);

        if (!result.Success)
            return BadRequest(result.Errors);

        return Ok(result.Data);
    }

    [HttpPatch("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCurrent(
        [FromQuery] Guid usuarioId,
        [FromBody] UpdateUsuarioRequest request,
        CancellationToken ct = default)
    {
        if (usuarioId == Guid.Empty)
            return BadRequest("UsuarioId é obrigatório.");

        var result = await _usuarioService.UpdateProfileAsync(usuarioId, request, ct);
        if (!result.Success)
            return BadRequest(result.Errors);

        return Ok(result.Data);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromQuery] Guid? requesterId,
        CancellationToken ct = default)
    {
        var result = await _usuarioService.GetByIdAsync(id, requesterId, ct);
        if (!result.Success)
            return NotFound(result.Errors);

        return Ok(result.Data);
    }

    [HttpPost("{usuarioId:guid}/jogos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddJogo(
        Guid usuarioId,
        [FromBody] AddUsuarioJogoRequest request,
        CancellationToken ct = default)
    {
        if (usuarioId == Guid.Empty)
            return BadRequest("UsuarioId é obrigatório.");

        var result = await _usuarioService.AddJogoAsync(usuarioId, request, ct);
        if (!result.Success)
            return BadRequest(result.Errors);

        return Ok();
    }

    [HttpGet("{usuarioId:guid}/jogos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListJogos(
        Guid usuarioId,
        CancellationToken ct = default)
    {
        if (usuarioId == Guid.Empty)
            return BadRequest("UsuarioId é obrigatório.");

        var result = await _usuarioService.ListJogosAsync(usuarioId, ct);
        if (!result.Success)
            return NotFound(result.Errors);

        return Ok(result.Data);
    }

    [HttpDelete("{usuarioId:guid}/jogos/{jogoId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveJogo(
        Guid usuarioId,
        long jogoId,
        CancellationToken ct = default)
    {
        if (usuarioId == Guid.Empty)
            return BadRequest("UsuarioId é obrigatório.");

        var result = await _usuarioService.RemoveJogoAsync(usuarioId, jogoId, ct);
        if (!result.Success)
            return BadRequest(result.Errors);

        return Ok();
    }

    [HttpPatch("{usuarioId:guid}/jogos/{jogoId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateJogo(
        Guid usuarioId,
        long jogoId,
        [FromBody] UpdateUsuarioJogoRequest request,
        CancellationToken ct = default)
    {
        if (usuarioId == Guid.Empty)
            return BadRequest("UsuarioId é obrigatório.");

        var result = await _usuarioService.UpdateJogoAsync(usuarioId, jogoId, request, ct);
        if (!result.Success)
            return BadRequest(result.Errors);

        return Ok();
    }
}
