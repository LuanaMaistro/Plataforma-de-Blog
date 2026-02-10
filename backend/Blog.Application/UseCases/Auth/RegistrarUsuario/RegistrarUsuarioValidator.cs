using FluentValidation;

namespace Blog.Application.UseCases.Auth.RegistrarUsuario;

public class RegistrarUsuarioValidator : AbstractValidator<RegistrarUsuarioCommand>
{
    public RegistrarUsuarioValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome e obrigatorio")
            .MinimumLength(3).WithMessage("Nome deve ter pelo menos 3 caracteres")
            .MaximumLength(100).WithMessage("Nome deve ter no maximo 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email e obrigatorio")
            .EmailAddress().WithMessage("Email invalido");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha e obrigatoria")
            .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres")
            .Matches("[A-Z]").WithMessage("Senha deve conter pelo menos uma letra maiuscula")
            .Matches("[0-9]").WithMessage("Senha deve conter pelo menos um numero");
    }
}
