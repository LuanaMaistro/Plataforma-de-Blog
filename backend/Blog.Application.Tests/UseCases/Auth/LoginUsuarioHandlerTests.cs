using Blog.Application.Interfaces;
using Blog.Application.UseCases.Auth.LoginUsuario;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Domain.Interfaces;
using Blog.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace Blog.Application.Tests.UseCases.Auth;

public class LoginUsuarioHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepoMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly LoginUsuarioHandler _handler;

    public LoginUsuarioHandlerTests()
    {
        _handler = new LoginUsuarioHandler(
            _usuarioRepoMock.Object,
            _passwordHasherMock.Object,
            _tokenServiceMock.Object);
    }

    private static Usuario CriarUsuario()
    {
        return new Usuario("João", new Email("joao@email.com"), new SenhaHash("hashed_password"));
    }

    [Fact]
    public async Task Handle_ComCredenciaisValidas_DeveRetornarToken()
    {
        var command = new LoginUsuarioCommand("joao@email.com", "Senha123");
        var usuario = CriarUsuario();

        _usuarioRepoMock.Setup(r => r.ObterPorEmailAsync("joao@email.com")).ReturnsAsync(usuario);
        _passwordHasherMock.Setup(h => h.Verificar("Senha123", "hashed_password")).Returns(true);
        _tokenServiceMock.Setup(t => t.GerarToken(usuario)).Returns("jwt_token");

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Token.Should().Be("jwt_token");
        result.Nome.Should().Be("João");
        result.Email.Should().Be("joao@email.com");
    }

    [Fact]
    public async Task Handle_ComEmailNaoEncontrado_DeveLancarBusinessRuleException()
    {
        var command = new LoginUsuarioCommand("naoexiste@email.com", "Senha123");
        _usuarioRepoMock.Setup(r => r.ObterPorEmailAsync("naoexiste@email.com")).ReturnsAsync((Usuario?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("Email ou senha inválidos");
    }

    [Fact]
    public async Task Handle_ComSenhaIncorreta_DeveLancarBusinessRuleException()
    {
        var command = new LoginUsuarioCommand("joao@email.com", "SenhaErrada");
        var usuario = CriarUsuario();

        _usuarioRepoMock.Setup(r => r.ObterPorEmailAsync("joao@email.com")).ReturnsAsync(usuario);
        _passwordHasherMock.Setup(h => h.Verificar("SenhaErrada", "hashed_password")).Returns(false);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("Email ou senha inválidos");
    }
}
