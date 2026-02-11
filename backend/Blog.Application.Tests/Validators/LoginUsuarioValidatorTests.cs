using Blog.Application.UseCases.Auth.LoginUsuario;
using FluentValidation.TestHelper;
using Xunit;

namespace Blog.Application.Tests.Validators;

public class LoginUsuarioValidatorTests
{
    private readonly LoginUsuarioValidator _validator = new();

    [Fact]
    public void Validar_ComDadosValidos_DevePassar()
    {
        var command = new LoginUsuarioCommand("joao@email.com", "Senha123");

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validar_EmailVazio_DeveFalhar()
    {
        var command = new LoginUsuarioCommand("", "Senha123");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("Email e obrigatorio");
    }

    [Fact]
    public void Validar_EmailInvalido_DeveFalhar()
    {
        var command = new LoginUsuarioCommand("emailinvalido", "Senha123");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("Email invalido");
    }

    [Fact]
    public void Validar_SenhaVazia_DeveFalhar()
    {
        var command = new LoginUsuarioCommand("joao@email.com", "");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Senha).WithErrorMessage("Senha e obrigatoria");
    }
}
