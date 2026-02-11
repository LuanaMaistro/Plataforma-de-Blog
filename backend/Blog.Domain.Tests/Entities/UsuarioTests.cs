using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Blog.Domain.Tests.Entities;

public class UsuarioTests
{
    private static Email CriarEmail(string email = "teste@email.com") => new(email);
    private static SenhaHash CriarSenha(string hash = "hash123") => new(hash);

    [Fact]
    public void Criar_ComDadosValidos_DeveCriarUsuario()
    {
        var email = CriarEmail();
        var senha = CriarSenha();

        var usuario = new Usuario("João Silva", email, senha);

        usuario.Nome.Should().Be("João Silva");
        usuario.Email.Should().Be(email);
        usuario.Senha.Should().Be(senha);
        usuario.DataCriacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        usuario.Postagens.Should().BeEmpty();
    }

    [Fact]
    public void Criar_ComNomeVazio_DeveLancarDomainException()
    {
        var act = () => new Usuario("", CriarEmail(), CriarSenha());

        act.Should().Throw<DomainException>().WithMessage("Nome e obrigatorio");
    }

    [Fact]
    public void Criar_ComNomeMenorQue3Caracteres_DeveLancarDomainException()
    {
        var act = () => new Usuario("AB", CriarEmail(), CriarSenha());

        act.Should().Throw<DomainException>().WithMessage("Nome deve ter pelo menos 3 caracteres");
    }

    [Fact]
    public void AtualizarNome_ComNomeValido_DeveAtualizar()
    {
        var usuario = new Usuario("João Silva", CriarEmail(), CriarSenha());

        usuario.AtualizarNome("Maria Santos");

        usuario.Nome.Should().Be("Maria Santos");
    }

    [Fact]
    public void AtualizarNome_ComNomeInvalido_DeveLancarDomainException()
    {
        var usuario = new Usuario("João Silva", CriarEmail(), CriarSenha());

        var act = () => usuario.AtualizarNome("AB");

        act.Should().Throw<DomainException>().WithMessage("Nome deve ter pelo menos 3 caracteres");
    }
}
