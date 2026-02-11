using Blog.Application.UseCases.Auth.RegistrarUsuario;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Blog.Application.Tests.Validators;

public class RegistrarUsuarioValidatorTests
{
    private readonly RegistrarUsuarioValidator _validator = new();

    [Fact]
    public void Validar_ComDadosValidos_DevePassar()
    {
        var command = new RegistrarUsuarioCommand("João Silva", "joao@email.com", "Senha123");

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validar_NomeVazio_DeveFalhar()
    {
        var command = new RegistrarUsuarioCommand("", "joao@email.com", "Senha123");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Nome).WithErrorMessage("Nome e obrigatorio");
    }

    [Fact]
    public void Validar_NomeCurto_DeveFalhar()
    {
        var command = new RegistrarUsuarioCommand("AB", "joao@email.com", "Senha123");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Nome).WithErrorMessage("Nome deve ter pelo menos 3 caracteres");
    }

    [Fact]
    public void Validar_EmailInvalido_DeveFalhar()
    {
        var command = new RegistrarUsuarioCommand("João", "emailinvalido", "Senha123");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("Email invalido");
    }

    [Fact]
    public void Validar_SenhaCurta_DeveFalhar()
    {
        var command = new RegistrarUsuarioCommand("João", "joao@email.com", "Se1");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Senha).WithErrorMessage("Senha deve ter pelo menos 6 caracteres");
    }

    [Fact]
    public void Validar_SenhaSemMaiuscula_DeveFalhar()
    {
        var command = new RegistrarUsuarioCommand("João", "joao@email.com", "senha123");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Senha).WithErrorMessage("Senha deve conter pelo menos uma letra maiuscula");
    }

    [Fact]
    public void Validar_SenhaSemNumero_DeveFalhar()
    {
        var command = new RegistrarUsuarioCommand("João", "joao@email.com", "Senhaaa");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Senha).WithErrorMessage("Senha deve conter pelo menos um numero");
    }
}
