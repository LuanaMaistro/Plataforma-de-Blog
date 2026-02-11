using AutoMapper;
using Blog.Application.DTOs.Postagem;
using Blog.Application.UseCases.Postagens.ObterPostagem;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Blog.Application.Tests.UseCases.Postagens;

public class ObterPostagemHandlerTests
{
    private readonly Mock<IPostagemRepository> _postagemRepoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly ObterPostagemHandler _handler;

    public ObterPostagemHandlerTests()
    {
        _handler = new ObterPostagemHandler(_postagemRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_PostagemExistente_DeveRetornarPostagem()
    {
        var query = new ObterPostagemQuery(1);
        var postagem = new Postagem("Título", "Conteúdo", 1);
        var expectedResponse = new PostagemResponseDto
        {
            Id = 1, Titulo = "Título", Conteudo = "Conteúdo"
        };

        _postagemRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(postagem);
        _mapperMock.Setup(m => m.Map<PostagemResponseDto>(postagem)).Returns(expectedResponse);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task Handle_PostagemNaoExistente_DeveRetornarNull()
    {
        var query = new ObterPostagemQuery(99);
        _postagemRepoMock.Setup(r => r.ObterPorIdAsync(99)).ReturnsAsync((Postagem?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }
}
