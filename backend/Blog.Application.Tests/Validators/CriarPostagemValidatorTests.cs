using Blog.Application.UseCases.Postagens.CriarPostagem;
using FluentValidation.TestHelper;
using Xunit;

namespace Blog.Application.Tests.Validators;

public class CriarPostagemValidatorTests
{
    private readonly CriarPostagemValidator _validator = new();

    [Fact]
    public void Validar_ComDadosValidos_DevePassar()
    {
        var command = new CriarPostagemCommand("Título do Post", "Conteúdo do post", 1);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validar_TituloVazio_DeveFalhar()
    {
        var command = new CriarPostagemCommand("", "Conteúdo", 1);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Titulo).WithErrorMessage("Titulo e obrigatorio");
    }

    [Fact]
    public void Validar_TituloMaiorQue200_DeveFalhar()
    {
        var tituloLongo = new string('A', 201);
        var command = new CriarPostagemCommand(tituloLongo, "Conteúdo", 1);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Titulo)
            .WithErrorMessage("Titulo deve ter no maximo 200 caracteres");
    }

    [Fact]
    public void Validar_ConteudoVazio_DeveFalhar()
    {
        var command = new CriarPostagemCommand("Título", "", 1);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Conteudo).WithErrorMessage("Conteudo e obrigatorio");
    }

    [Fact]
    public void Validar_UsuarioIdMenorOuIgualAZero_DeveFalhar()
    {
        var command = new CriarPostagemCommand("Título", "Conteúdo", 0);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UsuarioId).WithErrorMessage("Usuario invalido");
    }
}
