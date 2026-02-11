using Blog.Application.UseCases.Postagens.EditarPostagem;
using FluentValidation.TestHelper;
using Xunit;

namespace Blog.Application.Tests.Validators;

public class EditarPostagemValidatorTests
{
    private readonly EditarPostagemValidator _validator = new();

    [Fact]
    public void Validar_ComDadosValidos_DevePassar()
    {
        var command = new EditarPostagemCommand(1, "Título", "Conteúdo", 1);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validar_IdMenorOuIgualAZero_DeveFalhar()
    {
        var command = new EditarPostagemCommand(0, "Título", "Conteúdo", 1);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Id).WithErrorMessage("Id invalido");
    }

    [Fact]
    public void Validar_TituloVazio_DeveFalhar()
    {
        var command = new EditarPostagemCommand(1, "", "Conteúdo", 1);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Titulo).WithErrorMessage("Titulo e obrigatorio");
    }

    [Fact]
    public void Validar_ConteudoVazio_DeveFalhar()
    {
        var command = new EditarPostagemCommand(1, "Título", "", 1);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Conteudo).WithErrorMessage("Conteudo e obrigatorio");
    }
}
