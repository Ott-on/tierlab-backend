using TierLab.Application.Common;
using TierLab.Application.UseCases.Usuarios.Dtos;
using TierLab.Application.UseCases.Usuarios.Requests;
using TierLab.Domain.Entities;
using TierLab.Domain.Interfaces;

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
