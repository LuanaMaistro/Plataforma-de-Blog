using Blog.Domain.Exceptions;
using Blog.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Blog.Domain.Tests.ValueObjects;

public class SenhaHashTests
{
    [Fact]
    public void Criar_ComHashValido_DeveAceitar()
    {
        var senha = new SenhaHash("hash_valido_123");

        senha.Valor.Should().Be("hash_valido_123");
    }

    [Fact]
    public void Criar_ComHashVazio_DeveLancarDomainException()
    {
        var act = () => new SenhaHash("");

        act.Should().Throw<DomainException>().WithMessage("Hash de senha invalido");
    }

    [Fact]
    public void ToString_DeveRetornarAsteriscos()
    {
        var senha = new SenhaHash("hash_secreto");

        senha.ToString().Should().Be("****");
    }

    [Fact]
    public void Equals_EntreHashesIguais_DeveRetornarTrue()
    {
        var senha1 = new SenhaHash("hash_123");
        var senha2 = new SenhaHash("hash_123");

        senha1.Equals(senha2).Should().BeTrue();
    }

    [Fact]
    public void ConversaoImplicita_ParaString_DeveRetornarValor()
    {
        var senha = new SenhaHash("hash_123");

        string valor = senha;

        valor.Should().Be("hash_123");
    }
}
