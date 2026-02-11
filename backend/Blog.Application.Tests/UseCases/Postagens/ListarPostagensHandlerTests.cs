using AutoMapper;
using Blog.Application.DTOs.Postagem;
using Blog.Application.UseCases.Postagens.ListarPostagens;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Blog.Application.Tests.UseCases.Postagens;

public class ListarPostagensHandlerTests
{
    private readonly Mock<IPostagemRepository> _postagemRepoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly ListarPostagensHandler _handler;

    public ListarPostagensHandlerTests()
    {
        _handler = new ListarPostagensHandler(_postagemRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_SemUsuarioId_DeveListarTodas()
    {
        var query = new ListarPostagensQuery();
        var postagens = new List<Postagem>();
        var expectedResponse = new List<PostagemResponseDto>();

        _postagemRepoMock.Setup(r => r.ListarTodasAsync()).ReturnsAsync(postagens);
        _mapperMock.Setup(m => m.Map<IEnumerable<PostagemResponseDto>>(postagens)).Returns(expectedResponse);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedResponse);
        _postagemRepoMock.Verify(r => r.ListarTodasAsync(), Times.Once);
        _postagemRepoMock.Verify(r => r.ListarPorUsuarioAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ComUsuarioId_DeveListarPorUsuario()
    {
        var query = new ListarPostagensQuery(UsuarioId: 1);
        var postagens = new List<Postagem>();
        var expectedResponse = new List<PostagemResponseDto>();

        _postagemRepoMock.Setup(r => r.ListarPorUsuarioAsync(1)).ReturnsAsync(postagens);
        _mapperMock.Setup(m => m.Map<IEnumerable<PostagemResponseDto>>(postagens)).Returns(expectedResponse);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedResponse);
        _postagemRepoMock.Verify(r => r.ListarPorUsuarioAsync(1), Times.Once);
        _postagemRepoMock.Verify(r => r.ListarTodasAsync(), Times.Never);
    }
}
