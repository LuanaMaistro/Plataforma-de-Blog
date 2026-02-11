using Blog.Application.Interfaces;
using Blog.Application.UseCases.Auth.AtualizarPerfil;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Domain.Interfaces;
using Blog.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace Blog.Application.Tests.UseCases.Auth;

public class AtualizarPerfilHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepoMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly AtualizarPerfilHandler _handler;

    public AtualizarPerfilHandlerTests()
    {
        _handler = new AtualizarPerfilHandler(
            _usuarioRepoMock.Object,
            _passwordHasherMock.Object);
    }

    private static Usuario CriarUsuario()
    {
        return new Usuario("João", new Email("joao@email.com"), new SenhaHash("hashed_password"));
    }

    [Fact]
    public async Task Handle_AtualizarNome_DeveAtualizarComSucesso()
    {
        var usuario = CriarUsuario();
        var command = new AtualizarPerfilCommand(1, "Novo Nome", null, null);

        _usuarioRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(usuario);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Nome.Should().Be("Novo Nome");
        _usuarioRepoMock.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_AtualizarSenha_DeveAtualizarComSucesso()
    {
        var usuario = CriarUsuario();
        var command = new AtualizarPerfilCommand(1, "João", "SenhaAtual123", "NovaSenha123");

        _usuarioRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(usuario);
        _passwordHasherMock.Setup(h => h.Verificar("SenhaAtual123", "hashed_password")).Returns(true);
        _passwordHasherMock.Setup(h => h.Verificar("NovaSenha123", "hashed_password")).Returns(false);
        _passwordHasherMock.Setup(h => h.Hash("NovaSenha123")).Returns("new_hashed_password");

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        _usuarioRepoMock.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_UsuarioNaoEncontrado_DeveLancarBusinessRuleException()
    {
        var command = new AtualizarPerfilCommand(99, "Nome", null, null);
        _usuarioRepoMock.Setup(r => r.ObterPorIdAsync(99)).ReturnsAsync((Usuario?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("Usuario nao encontrado");
    }

    [Fact]
    public async Task Handle_SenhaAtualIncorreta_DeveLancarBusinessRuleException()
    {
        var usuario = CriarUsuario();
        var command = new AtualizarPerfilCommand(1, "João", "SenhaErrada", "NovaSenha123");

        _usuarioRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(usuario);
        _passwordHasherMock.Setup(h => h.Verificar("SenhaErrada", "hashed_password")).Returns(false);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("Senha atual incorreta");
    }

    [Fact]
    public async Task Handle_NovaSenhaIgualAtual_DeveLancarBusinessRuleException()
    {
        var usuario = CriarUsuario();
        var command = new AtualizarPerfilCommand(1, "João", "SenhaAtual123", "SenhaAtual123");

        _usuarioRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(usuario);
        _passwordHasherMock.Setup(h => h.Verificar("SenhaAtual123", "hashed_password")).Returns(true);
        _passwordHasherMock.Setup(h => h.Verificar("SenhaAtual123", "hashed_password")).Returns(true);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("A nova senha deve ser diferente da senha atual");
    }
}
