using AutoMapper;
using Blog.Application.DTOs.Postagem;
using Blog.Application.UseCases.Postagens.EditarPostagem;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Blog.Application.Tests.UseCases.Postagens;

public class EditarPostagemHandlerTests
{
    private readonly Mock<IPostagemRepository> _postagemRepoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly EditarPostagemHandler _handler;

    public EditarPostagemHandlerTests()
    {
        _handler = new EditarPostagemHandler(_postagemRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ComDadosValidos_DeveEditarPostagem()
    {
        var command = new EditarPostagemCommand(1, "Novo Título", "Novo Conteúdo", 1);
        var postagem = new Postagem("Título", "Conteúdo", 1);
        var expectedResponse = new PostagemResponseDto
        {
            Id = 1, Titulo = "Novo Título", Conteudo = "Novo Conteúdo"
        };

        _postagemRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(postagem);
        _mapperMock.Setup(m => m.Map<PostagemResponseDto>(postagem)).Returns(expectedResponse);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Titulo.Should().Be("Novo Título");
        _postagemRepoMock.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_PostagemNaoEncontrada_DeveLancarBusinessRuleException()
    {
        var command = new EditarPostagemCommand(99, "Título", "Conteúdo", 1);
        _postagemRepoMock.Setup(r => r.ObterPorIdAsync(99)).ReturnsAsync((Postagem?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("Postagem nao encontrada");
    }

    [Fact]
    public async Task Handle_NaoEDonoDaPostagem_DeveLancarBusinessRuleException()
    {
        var command = new EditarPostagemCommand(1, "Título", "Conteúdo", 2);
        var postagem = new Postagem("Título", "Conteúdo", 1);

        _postagemRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(postagem);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Voce nao tem permissao para editar esta postagem");
    }
}
