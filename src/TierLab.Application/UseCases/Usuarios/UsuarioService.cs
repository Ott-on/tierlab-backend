using TierLab.Application.Common;
using TierLab.Application.UseCases.Usuarios.Dtos;
using TierLab.Application.UseCases.Usuarios.Requests;
using TierLab.Domain.Entities;
using TierLab.Domain.Interfaces;
using TierLab.Application.UseCases.Usuarios.Dtos;

namespace TierLab.Application.UseCases.Usuarios;

public sealed class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        IUnitOfWork unitOfWork)
    {
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UsuarioProfileDto>> GetOrCreateCurrentAsync(
        Guid id,
        string? email,
        string? username,
        string? imageUrl,
        CancellationToken ct = default)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id, ct);

        if (usuario is null)
        {
            usuario = new Usuario
            {
                Id = id,
                Email = email,
                Username = BuildUsername(username, email),
                ImageUrl = imageUrl,
                Role = "user",
                PerfilPublico = true,
                Bio = null
            };

            await _usuarioRepository.AddAsync(usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<UsuarioProfileDto>.Ok(MapToDto(usuario));
        }

        var modified = false;

        if (!string.Equals(usuario.Email, email, StringComparison.OrdinalIgnoreCase))
        {
            usuario.Email = email;
            modified = true;
        }

        var normalizedUsername = BuildUsername(username, email);
        if (!string.IsNullOrWhiteSpace(normalizedUsername) &&
            !string.Equals(usuario.Username, normalizedUsername, StringComparison.Ordinal))
        {
            usuario.Username = normalizedUsername;
            modified = true;
        }

        if (!string.Equals(usuario.ImageUrl, imageUrl, StringComparison.Ordinal))
        {
            usuario.ImageUrl = imageUrl;
            modified = true;
        }

        if (modified)
        {
            await _usuarioRepository.UpdateAsync(usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        return Result<UsuarioProfileDto>.Ok(MapToDto(usuario));
    }

    public async Task<Result<UsuarioProfileDto>> GetByIdAsync(Guid id, Guid? requesterId, CancellationToken ct = default)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id, ct);

        if (usuario is null)
            return Result<UsuarioProfileDto>.Fail("Usuário não encontrado.");

        if (requesterId.HasValue && requesterId.Value == usuario.Id)
            return Result<UsuarioProfileDto>.Ok(MapToDto(usuario));

        if (!usuario.PerfilPublico)
            return Result<UsuarioProfileDto>.Fail("Perfil não público.");

        return Result<UsuarioProfileDto>.Ok(MapToDto(usuario));
    }

    public async Task<Result<UsuarioProfileDto>> UpdateProfileAsync(Guid id, UpdateUsuarioRequest request, CancellationToken ct = default)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id, ct);

        if (usuario is null)
            return Result<UsuarioProfileDto>.Fail("Usuário não encontrado.");

        var modified = false;

        if (!string.IsNullOrWhiteSpace(request.Username) &&
            !string.Equals(usuario.Username, request.Username, StringComparison.Ordinal))
        {
            usuario.Username = request.Username.Trim();
            modified = true;
        }

        if (request.ImageUrl is not null &&
            !string.Equals(usuario.ImageUrl, request.ImageUrl, StringComparison.Ordinal))
        {
            usuario.ImageUrl = request.ImageUrl;
            modified = true;
        }

        if (request.Bio is not null &&
            !string.Equals(usuario.Bio, request.Bio, StringComparison.Ordinal))
        {
            usuario.Bio = request.Bio;
            modified = true;
        }

        if (request.PerfilPublico.HasValue && usuario.PerfilPublico != request.PerfilPublico.Value)
        {
            usuario.PerfilPublico = request.PerfilPublico.Value;
            modified = true;
        }

        if (modified)
        {
            await _usuarioRepository.UpdateAsync(usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        return Result<UsuarioProfileDto>.Ok(MapToDto(usuario));
    }

    public async Task<Result> AddJogoAsync(Guid usuarioId, AddUsuarioJogoRequest request, CancellationToken ct = default)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(usuarioId, ct);
        if (usuario is null)
            return Result.Fail("Usuário não encontrado.");

        if (request.JogoId <= 0)
            return Result.Fail("JogoId inválido.");

        if (!await _usuarioRepository.JogoExistsAsync(request.JogoId, ct))
            return Result.Fail("Jogo não encontrado.");

        if (await _usuarioRepository.UsuarioJogoExistsAsync(usuarioId, request.JogoId, ct))
            return Result.Fail("Jogo já adicionado à coleção do usuário.");

        var usuarioJogo = new UsuarioJogo
        {
            UsuarioId = usuarioId,
            JogoId = request.JogoId,
            Status = string.IsNullOrWhiteSpace(request.Status) ? null : request.Status.Trim(),
            Nota = request.Nota,
            HorasJogadas = request.HorasJogadas ?? 0,
            Usuario = usuario
        };

        await _usuarioRepository.AddUsuarioJogoAsync(usuarioJogo, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok();
    }

    public async Task<Result<IReadOnlyList<UsuarioJogoResponse>>> ListJogosAsync(Guid usuarioId, CancellationToken ct = default)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(usuarioId, ct);
        if (usuario is null)
            return Result<IReadOnlyList<UsuarioJogoResponse>>.Fail("Usuário não encontrado.");

        var items = await _usuarioRepository.GetUsuarioJogosAsync(usuarioId, ct);
        var dtos = items.Select(uj => new UsuarioJogoResponse(
            uj.JogoId,
            uj.Status,
            uj.Nota,
            uj.HorasJogadas,
            uj.AdicionadoEm)).ToList();

        return Result<IReadOnlyList<UsuarioJogoResponse>>.Ok(dtos);
    }

    public async Task<Result> RemoveJogoAsync(Guid usuarioId, long jogoId, CancellationToken ct = default)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(usuarioId, ct);
        if (usuario is null)
            return Result.Fail("Usuário não encontrado.");

        var existing = await _usuarioRepository.GetUsuarioJogoAsync(usuarioId, jogoId, ct);
        if (existing is null)
            return Result.Fail("Jogo não está presente na coleção do usuário.");

        await _usuarioRepository.RemoveUsuarioJogoAsync(existing, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok();
    }

    public async Task<Result> UpdateJogoAsync(Guid usuarioId, long jogoId, UpdateUsuarioJogoRequest request, CancellationToken ct = default)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(usuarioId, ct);
        if (usuario is null)
            return Result.Fail("Usuário não encontrado.");

        var existing = await _usuarioRepository.GetUsuarioJogoAsync(usuarioId, jogoId, ct);
        if (existing is null)
            return Result.Fail("Jogo não está presente na coleção do usuário.");

        var modified = false;

        if (request.Status is not null && !string.Equals(existing.Status, request.Status, StringComparison.Ordinal))
        {
            existing.Status = string.IsNullOrWhiteSpace(request.Status) ? null : request.Status.Trim();
            modified = true;
        }

        if (request.Nota.HasValue && existing.Nota != request.Nota.Value)
        {
            existing.Nota = request.Nota;
            modified = true;
        }

        if (request.HorasJogadas.HasValue && existing.HorasJogadas != request.HorasJogadas.Value)
        {
            existing.HorasJogadas = request.HorasJogadas.Value;
            modified = true;
        }

        if (modified)
        {
            await _usuarioRepository.UpdateUsuarioJogoAsync(existing, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        return Result.Ok();
    }

    private static string BuildUsername(string? username, string? email)
    {
        if (!string.IsNullOrWhiteSpace(username))
            return username.Trim();

        if (!string.IsNullOrWhiteSpace(email))
        {
            var atIndex = email.IndexOf('@');
            var localPart = atIndex > 0 ? email[..atIndex] : email;
            return localPart.Trim();
        }

        return $"user-{Guid.NewGuid():N}";
    }

    private static UsuarioProfileDto MapToDto(Usuario usuario)
        => new(
            usuario.Id,
            usuario.Username,
            usuario.Email,
            usuario.ImageUrl,
            usuario.Bio,
            usuario.PerfilPublico,
            usuario.Role,
            usuario.CriadoEm);
}
