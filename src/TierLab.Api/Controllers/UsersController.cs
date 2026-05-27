using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TierLab.Api.Extensions;
using TierLab.Application.UseCases.Usuarios;
using TierLab.Application.UseCases.Usuarios.Requests;

namespace TierLab.Api.Controllers;

public sealed class UsersController : BaseController
{
    private readonly IUsuarioService _usuarioService;

    public UsersController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrent(CancellationToken ct = default)
    {
        var userId = User.GetSupabaseUserId();
        if (userId is null)
            return Unauthorized();

        var result = await _usuarioService.GetOrCreateCurrentAsync(
            userId.Value,
            User.GetSupabaseEmail(),
            User.GetSupabaseUsername(),
            User.GetSupabasePicture(),
            ct);

        if (!result.Success)
            return BadRequest(result.Errors);

        return Ok(result.Data);
    }

    [Authorize]
    [HttpPatch("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateCurrent(
        [FromBody] UpdateUsuarioRequest request,
        CancellationToken ct = default)
    {
        var userId = User.GetSupabaseUserId();
        if (userId is null)
            return Unauthorized();

        var result = await _usuarioService.UpdateProfileAsync(userId.Value, request, ct);
        if (!result.Success)
            return BadRequest(result.Errors);

        return Ok(result.Data);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var requesterId = User.GetSupabaseUserId();

        var result = await _usuarioService.GetByIdAsync(id, requesterId, ct);
        if (!result.Success)
            return NotFound(result.Errors);

        return Ok(result.Data);
    }
}
