using AutoMapper;
using Blog.Application.DTOs.Postagem;
using Blog.Application.UseCases.Postagens.CriarPostagem;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace Blog.Application.Tests.UseCases.Postagens;

public class CriarPostagemHandlerTests
{
    private readonly Mock<IPostagemRepository> _postagemRepoMock = new();
    private readonly Mock<IUsuarioRepository> _usuarioRepoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly CriarPostagemHandler _handler;

    public CriarPostagemHandlerTests()
    {
        _handler = new CriarPostagemHandler(
            _postagemRepoMock.Object,
            _usuarioRepoMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ComDadosValidos_DeveCriarPostagem()
    {
        var command = new CriarPostagemCommand("Título", "Conteúdo", 1);
        var usuario = new Usuario("João", new Email("joao@email.com"), new SenhaHash("hash"));

        _usuarioRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(usuario);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Titulo.Should().Be("Título");
        result.Conteudo.Should().Be("Conteúdo");
        _postagemRepoMock.Verify(r => r.AdicionarAsync(It.IsAny<Postagem>()), Times.Once);
        _postagemRepoMock.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }
}
