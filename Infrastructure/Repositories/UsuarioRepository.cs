using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync(CancellationToken ct)
        => await _context.Usuarios.AsNoTracking().ToListAsync(ct);

    public async Task<Usuario?> GetByIdAsync(int id, CancellationToken ct)
        => await _context.Usuarios.FindAsync(new object[] { id }, ct);

    public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct)
        => await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task AddAsync(Usuario usuario, CancellationToken ct)
        => await _context.Usuarios.AddAsync(usuario, ct);

    public Task UpdateAsync(Usuario usuario, CancellationToken ct)
    {
        _context.Usuarios.Update(usuario);
        return Task.CompletedTask;
    }

    // Soft delete: o service muda Ativo = false,
    // aqui só marcamos como modificado.
    public Task RemoveAsync(Usuario usuario, CancellationToken ct)
    {
        _context.Usuarios.Update(usuario);
        return Task.CompletedTask;
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct)
        => await _context.Usuarios.AnyAsync(u => u.Email == email, ct);

    public Task<int> SaveChangesAsync(CancellationToken ct)
        => _context.SaveChangesAsync(ct);
}
