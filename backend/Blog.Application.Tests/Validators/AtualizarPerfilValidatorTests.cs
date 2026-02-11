using Blog.Application.UseCases.Auth.AtualizarPerfil;
using FluentValidation.TestHelper;
using Xunit;

namespace Blog.Application.Tests.Validators;

public class AtualizarPerfilValidatorTests
{
    private readonly AtualizarPerfilValidator _validator = new();

    [Fact]
    public void Validar_ApenasNome_DevePassar()
    {
        var command = new AtualizarPerfilCommand(1, "Novo Nome", null, null);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validar_NomeESenha_DevePassar()
    {
        var command = new AtualizarPerfilCommand(1, "Novo Nome", "SenhaAtual1", "NovaSenha1");

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validar_NomeVazio_DeveFalhar()
    {
        var command = new AtualizarPerfilCommand(1, "", null, null);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Nome).WithErrorMessage("Nome e obrigatorio");
    }

    [Fact]
    public void Validar_SenhaAtualVaziaQuandoNovaSenhaInformada_DeveFalhar()
    {
        var command = new AtualizarPerfilCommand(1, "Nome", "", "NovaSenha1");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.SenhaAtual)
            .WithErrorMessage("Senha atual e obrigatoria para alterar a senha");
    }

    [Fact]
    public void Validar_NovaSenhaCurta_DeveFalhar()
    {
        var command = new AtualizarPerfilCommand(1, "Nome", "SenhaAtual1", "No1");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.NovaSenha)
            .WithErrorMessage("Nova senha deve ter pelo menos 6 caracteres");
    }
}
