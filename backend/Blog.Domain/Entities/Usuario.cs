using Blog.Domain.Exceptions;
using Blog.Domain.ValueObjects;

namespace Blog.Domain.Entities;

public class Usuario
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public SenhaHash Senha { get; private set; } = null!;
    public ICollection<Postagem> Postagens { get; private set; } = new List<Postagem>();
    public DateTime DataCriacao { get; private set; }

    private Usuario() { } // EF Core

    public Usuario(string nome, Email email, SenhaHash senha)
    {
        ValidarNome(nome);
        Nome = nome;
        Email = email;
        Senha = senha;
        DataCriacao = DateTime.UtcNow;
        Postagens = new List<Postagem>();
    }

    public void AtualizarNome(string nome)
    {
        ValidarNome(nome);
        Nome = nome;
    }

    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome e obrigatorio");
        if (nome.Length < 3)
            throw new DomainException("Nome deve ter pelo menos 3 caracteres");
    }
}
