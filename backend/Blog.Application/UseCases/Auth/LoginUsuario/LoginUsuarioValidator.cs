using FluentValidation;

namespace Blog.Application.UseCases.Auth.LoginUsuario;

public class LoginUsuarioValidator : AbstractValidator<LoginUsuarioCommand>
{
    public LoginUsuarioValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email e obrigatorio")
            .EmailAddress().WithMessage("Email invalido");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha e obrigatoria");
    }
}
