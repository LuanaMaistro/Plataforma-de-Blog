using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Blog.Domain.Tests.Entities;

public class PostagemTests
{
    [Fact]
    public void Criar_ComDadosValidos_DeveCriarPostagem()
    {
        var postagem = new Postagem("Título", "Conteúdo do post", 1);

        postagem.Titulo.Should().Be("Título");
        postagem.Conteudo.Should().Be("Conteúdo do post");
        postagem.UsuarioId.Should().Be(1);
        postagem.DataCriacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        postagem.DataAtualizacao.Should().BeNull();
    }

    [Fact]
    public void Criar_ComTituloVazio_DeveLancarDomainException()
    {
        var act = () => new Postagem("", "Conteúdo", 1);

        act.Should().Throw<DomainException>().WithMessage("Titulo e obrigatorio");
    }

    [Fact]
    public void Criar_ComConteudoVazio_DeveLancarDomainException()
    {
        var act = () => new Postagem("Título", "", 1);

        act.Should().Throw<DomainException>().WithMessage("Conteudo e obrigatorio");
    }

    [Fact]
    public void Atualizar_ComDadosValidos_DeveAtualizar()
    {
        var postagem = new Postagem("Título", "Conteúdo", 1);

        postagem.Atualizar("Novo Título", "Novo Conteúdo");

        postagem.Titulo.Should().Be("Novo Título");
        postagem.Conteudo.Should().Be("Novo Conteúdo");
    }

    [Fact]
    public void Atualizar_DeveDefinirDataAtualizacao()
    {
        var postagem = new Postagem("Título", "Conteúdo", 1);

        postagem.Atualizar("Novo Título", "Novo Conteúdo");

        postagem.DataAtualizacao.Should().NotBeNull();
        postagem.DataAtualizacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void PertenceAoUsuario_ComMesmoUsuarioId_DeveRetornarTrue()
    {
        var postagem = new Postagem("Título", "Conteúdo", 1);

        postagem.PertenceAoUsuario(1).Should().BeTrue();
    }

    [Fact]
    public void PertenceAoUsuario_ComUsuarioIdDiferente_DeveRetornarFalse()
    {
        var postagem = new Postagem("Título", "Conteúdo", 1);

        postagem.PertenceAoUsuario(2).Should().BeFalse();
    }
}
