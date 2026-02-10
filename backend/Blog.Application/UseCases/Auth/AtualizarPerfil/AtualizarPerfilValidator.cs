using FluentValidation;

namespace Blog.Application.UseCases.Auth.AtualizarPerfil;

public class AtualizarPerfilValidator : AbstractValidator<AtualizarPerfilCommand>
{
    public AtualizarPerfilValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome e obrigatorio")
            .MinimumLength(3).WithMessage("Nome deve ter pelo menos 3 caracteres");

        When(x => !string.IsNullOrEmpty(x.NovaSenha), () =>
        {
            RuleFor(x => x.SenhaAtual)
                .NotEmpty().WithMessage("Senha atual e obrigatoria para alterar a senha");

            RuleFor(x => x.NovaSenha)
                .MinimumLength(6).WithMessage("Nova senha deve ter pelo menos 6 caracteres");
        });
    }
}
