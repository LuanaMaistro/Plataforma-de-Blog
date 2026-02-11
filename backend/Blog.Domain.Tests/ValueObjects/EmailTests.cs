using Blog.Domain.Exceptions;
using Blog.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Blog.Domain.Tests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Criar_ComEmailValido_DeveAceitarEConverterParaLowercase()
    {
        var email = new Email("Teste@Email.COM");

        email.Valor.Should().Be("teste@email.com");
    }

    [Fact]
    public void Criar_ComEmailVazio_DeveLancarDomainException()
    {
        var act = () => new Email("");

        act.Should().Throw<DomainException>().WithMessage("Email invalido");
    }

    [Fact]
    public void Criar_ComEmailSemArroba_DeveLancarDomainException()
    {
        var act = () => new Email("emailsemarroba.com");

        act.Should().Throw<DomainException>().WithMessage("Email invalido");
    }

    [Fact]
    public void Criar_ComEmailTerminandoComPonto_DeveLancarDomainException()
    {
        var act = () => new Email("teste@email.com.");

        act.Should().Throw<DomainException>().WithMessage("Email invalido");
    }

    [Fact]
    public void Equals_EntreEmailsIguais_DeveRetornarTrue()
    {
        var email1 = new Email("teste@email.com");
        var email2 = new Email("teste@email.com");

        email1.Equals(email2).Should().BeTrue();
    }

    [Fact]
    public void ConversaoImplicita_ParaString_DeveRetornarValor()
    {
        var email = new Email("teste@email.com");

        string valor = email;

        valor.Should().Be("teste@email.com");
    }
}
