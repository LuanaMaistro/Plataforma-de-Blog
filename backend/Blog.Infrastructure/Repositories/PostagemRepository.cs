using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

public class PostagemRepository : IPostagemRepository
{
    private readonly BlogDbContext _context;

    public PostagemRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<Postagem?> ObterPorIdAsync(int id)
    {
        return await _context.Postagens
            .Include(p => p.Usuario)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Postagem>> ListarTodasAsync()
    {
        return await _context.Postagens
            .Include(p => p.Usuario)
            .OrderByDescending(p => p.DataCriacao)
            .ToListAsync();
    }

    public async Task<IEnumerable<Postagem>> ListarPorUsuarioAsync(int usuarioId)
    {
        return await _context.Postagens
            .Include(p => p.Usuario)
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.DataCriacao)
            .ToListAsync();
    }

    public async Task AdicionarAsync(Postagem postagem)
    {
        await _context.Postagens.AddAsync(postagem);
    }

    public async Task RemoverAsync(Postagem postagem)
    {
        _context.Postagens.Remove(postagem);
        await Task.CompletedTask;
    }

    public async Task SalvarAlteracoesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
