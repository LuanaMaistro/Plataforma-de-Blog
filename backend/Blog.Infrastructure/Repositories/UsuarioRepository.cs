using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly BlogDbContext _context;

    public UsuarioRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObterPorIdAsync(int id)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        var emailLower = email.ToLowerInvariant();
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email.Valor == emailLower);
    }

    public async Task<bool> ExisteEmailAsync(string email)
    {
        var emailLower = email.ToLowerInvariant();
        return await _context.Usuarios
            .AnyAsync(u => u.Email.Valor == emailLower);
    }

    public async Task AdicionarAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public async Task SalvarAlteracoesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
