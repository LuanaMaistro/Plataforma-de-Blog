using FluentValidation;

namespace Blog.Application.UseCases.Postagens.CriarPostagem;

public class CriarPostagemValidator : AbstractValidator<CriarPostagemCommand>
{
    public CriarPostagemValidator()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("Titulo e obrigatorio")
            .MaximumLength(200).WithMessage("Titulo deve ter no maximo 200 caracteres");

        RuleFor(x => x.Conteudo)
            .NotEmpty().WithMessage("Conteudo e obrigatorio");

        RuleFor(x => x.UsuarioId)
            .GreaterThan(0).WithMessage("Usuario invalido");
    }
}
