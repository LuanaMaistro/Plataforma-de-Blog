using Blog.Application.DTOs.Usuario;
using Blog.Application.Interfaces;
using Blog.Domain.Exceptions;
using Blog.Domain.Interfaces;
using MediatR;

namespace Blog.Application.UseCases.Auth.LoginUsuario;

public class LoginUsuarioHandler : IRequestHandler<LoginUsuarioCommand, LoginResponseDto>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginUsuarioHandler(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> Handle(LoginUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email.ToLowerInvariant());

        if (usuario is null)
            throw new BusinessRuleException("Email ou senha inválidos");

        if (!_passwordHasher.Verificar(request.Senha, usuario.Senha.Valor))
            throw new BusinessRuleException("Email ou senha inválidos");

        var token = _tokenService.GerarToken(usuario);

        return new LoginResponseDto(
            usuario.Id,
            usuario.Nome,
            usuario.Email.Valor,
            token
        );
    }
}
