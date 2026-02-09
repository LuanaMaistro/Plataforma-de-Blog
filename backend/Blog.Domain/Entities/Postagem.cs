using Blog.Domain.Exceptions;

namespace Blog.Domain.Entities;

public class Postagem
{
    public int Id { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public string Conteudo { get; private set; } = string.Empty;
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataAtualizacao { get; private set; }
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;

    private Postagem() { } // EF Core

    public Postagem(string titulo, string conteudo, int usuarioId)
    {
        Validar(titulo, conteudo);
        Titulo = titulo;
        Conteudo = conteudo;
        UsuarioId = usuarioId;
        DataCriacao = DateTime.UtcNow;
    }

    public void Atualizar(string titulo, string conteudo)
    {
        Validar(titulo, conteudo);
        Titulo = titulo;
        Conteudo = conteudo;
        DataAtualizacao = DateTime.UtcNow;
    }

    public bool PertenceAoUsuario(int usuarioId) => UsuarioId == usuarioId;

    private static void Validar(string titulo, string conteudo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("Titulo e obrigatorio");
        if (string.IsNullOrWhiteSpace(conteudo))
            throw new DomainException("Conteudo e obrigatorio");
    }
}
