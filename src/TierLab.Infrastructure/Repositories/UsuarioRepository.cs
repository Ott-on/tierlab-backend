using Microsoft.EntityFrameworkCore;
using TierLab.Domain.Entities;
using TierLab.Domain.Interfaces;
using TierLab.Infrastructure.Persistence;

namespace TierLab.Infrastructure.Repositories;

public sealed class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<bool> JogoExistsAsync(long jogoId, CancellationToken cancellationToken = default)
        => await _context.Jogos.AnyAsync(j => j.Id == jogoId, cancellationToken);

    public async Task<bool> UsuarioJogoExistsAsync(Guid usuarioId, long jogoId, CancellationToken cancellationToken = default)
        => await _context.UsuarioJogos.AnyAsync(uj => uj.UsuarioId == usuarioId && uj.JogoId == jogoId, cancellationToken);

    public async Task AddUsuarioJogoAsync(UsuarioJogo usuarioJogo, CancellationToken cancellationToken = default)
    {
        await _context.UsuarioJogos.AddAsync(usuarioJogo, cancellationToken);
    }

    public async Task<IReadOnlyList<UsuarioJogo>> GetUsuarioJogosAsync(Guid usuarioId, CancellationToken cancellationToken = default)
        => await _context.UsuarioJogos
            .AsNoTracking()
            .Where(uj => uj.UsuarioId == usuarioId)
            .ToListAsync(cancellationToken);

    public async Task<UsuarioJogo?> GetUsuarioJogoAsync(Guid usuarioId, long jogoId, CancellationToken cancellationToken = default)
        => await _context.UsuarioJogos.FindAsync(new object[] { usuarioId, jogoId }, cancellationToken);

    public Task RemoveUsuarioJogoAsync(UsuarioJogo usuarioJogo, CancellationToken cancellationToken = default)
    {
        _context.UsuarioJogos.Remove(usuarioJogo);
        return Task.CompletedTask;
    }

    public Task UpdateUsuarioJogoAsync(UsuarioJogo usuarioJogo, CancellationToken cancellationToken = default)
    {
        _context.UsuarioJogos.Update(usuarioJogo);
        return Task.CompletedTask;
    }

    public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task<Usuario> AddAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        var entry = await _context.Usuarios.AddAsync(usuario, cancellationToken);
        return entry.Entity;
    }

    public Task UpdateAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        _context.Usuarios.Update(usuario);
        return Task.CompletedTask;
    }
}
