using AutoMapper;
using Blog.Application.DTOs.Usuario;
using Blog.Application.Interfaces;
using Blog.Application.UseCases.Auth.RegistrarUsuario;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Blog.Application.Tests.UseCases.Auth;

public class RegistrarUsuarioHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepoMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly RegistrarUsuarioHandler _handler;

    public RegistrarUsuarioHandlerTests()
    {
        _handler = new RegistrarUsuarioHandler(
            _usuarioRepoMock.Object,
            _passwordHasherMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ComDadosValidos_DeveRegistrarUsuario()
    {
        var command = new RegistrarUsuarioCommand("João", "joao@email.com", "Senha123");
        var expectedResponse = new UsuarioResponseDto
        {
            Id = 1, Nome = "João", Email = "joao@email.com", DataCriacao = DateTime.UtcNow
        };

        _usuarioRepoMock.Setup(r => r.ExisteEmailAsync("joao@email.com")).ReturnsAsync(false);
        _passwordHasherMock.Setup(h => h.Hash("Senha123")).Returns("hashed_password");
        _mapperMock.Setup(m => m.Map<UsuarioResponseDto>(It.IsAny<Usuario>())).Returns(expectedResponse);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().Be(expectedResponse);
        _usuarioRepoMock.Verify(r => r.AdicionarAsync(It.IsAny<Usuario>()), Times.Once);
        _usuarioRepoMock.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ComEmailExistente_DeveLancarBusinessRuleException()
    {
        var command = new RegistrarUsuarioCommand("João", "joao@email.com", "Senha123");
        _usuarioRepoMock.Setup(r => r.ExisteEmailAsync("joao@email.com")).ReturnsAsync(true);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("Email ja cadastrado");
    }
}
