using Blog.Application.UseCases.Postagens.DeletarPostagem;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Blog.Application.Tests.UseCases.Postagens;

public class DeletarPostagemHandlerTests
{
    private readonly Mock<IPostagemRepository> _postagemRepoMock = new();
    private readonly DeletarPostagemHandler _handler;

    public DeletarPostagemHandlerTests()
    {
        _handler = new DeletarPostagemHandler(_postagemRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ComDadosValidos_DeveDeletarPostagem()
    {
        var command = new DeletarPostagemCommand(1, 1);
        var postagem = new Postagem("Título", "Conteúdo", 1);

        _postagemRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(postagem);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        _postagemRepoMock.Verify(r => r.RemoverAsync(postagem), Times.Once);
        _postagemRepoMock.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_PostagemNaoEncontrada_DeveLancarBusinessRuleException()
    {
        var command = new DeletarPostagemCommand(99, 1);
        _postagemRepoMock.Setup(r => r.ObterPorIdAsync(99)).ReturnsAsync((Postagem?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("Postagem nao encontrada");
    }

    [Fact]
    public async Task Handle_NaoEDonoDaPostagem_DeveLancarBusinessRuleException()
    {
        var command = new DeletarPostagemCommand(1, 2);
        var postagem = new Postagem("Título", "Conteúdo", 1);

        _postagemRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(postagem);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Voce nao tem permissao para deletar esta postagem");
    }
}
