using Blog.Application.DTOs.Usuario;
using Blog.Application.Interfaces;
using Blog.Domain.Exceptions;
using Blog.Domain.Interfaces;
using Blog.Domain.ValueObjects;
using MediatR;

namespace Blog.Application.UseCases.Auth.AtualizarPerfil;

public class AtualizarPerfilHandler : IRequestHandler<AtualizarPerfilCommand, UsuarioResponseDto>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AtualizarPerfilHandler(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UsuarioResponseDto> Handle(AtualizarPerfilCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(request.UsuarioId)
            ?? throw new BusinessRuleException("Usuario nao encontrado");

        usuario.AtualizarNome(request.Nome);

        if (!string.IsNullOrEmpty(request.NovaSenha))
        {
            if (!_passwordHasher.Verificar(request.SenhaAtual!, usuario.Senha.Valor))
                throw new BusinessRuleException("Senha atual incorreta");

            if (_passwordHasher.Verificar(request.NovaSenha, usuario.Senha.Valor))
                throw new BusinessRuleException("A nova senha deve ser diferente da senha atual");

            var novaHash = _passwordHasher.Hash(request.NovaSenha);
            usuario.AtualizarSenha(new SenhaHash(novaHash));
        }

        await _usuarioRepository.SalvarAlteracoesAsync();

        return new UsuarioResponseDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email.Valor,
            DataCriacao = usuario.DataCriacao
        };
    }
}
