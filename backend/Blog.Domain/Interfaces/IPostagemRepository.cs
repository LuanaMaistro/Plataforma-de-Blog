using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces;

public interface IPostagemRepository
{
    Task<Postagem?> ObterPorIdAsync(int id);
    Task<IEnumerable<Postagem>> ListarTodasAsync();
    Task<IEnumerable<Postagem>> ListarPorUsuarioAsync(int usuarioId);
    Task AdicionarAsync(Postagem postagem);
    Task RemoverAsync(Postagem postagem);
    Task SalvarAlteracoesAsync();
}
