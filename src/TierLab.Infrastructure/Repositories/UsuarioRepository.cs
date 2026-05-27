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
