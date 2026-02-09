using AutoMapper;
using Blog.Application.DTOs.Usuario;
using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Domain.Interfaces;
using Blog.Domain.ValueObjects;
using MediatR;

namespace Blog.Application.UseCases.Auth.RegistrarUsuario;

public class RegistrarUsuarioHandler : IRequestHandler<RegistrarUsuarioCommand, UsuarioResponseDto>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;

    public RegistrarUsuarioHandler(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher passwordHasher,
        IMapper mapper)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<UsuarioResponseDto> Handle(RegistrarUsuarioCommand request, CancellationToken cancellationToken)
    {
        if (await _usuarioRepository.ExisteEmailAsync(request.Email))
            throw new BusinessRuleException("Email ja cadastrado");

        var email = new Email(request.Email);
        var senhaHash = new SenhaHash(_passwordHasher.Hash(request.Senha));
        var usuario = new Usuario(request.Nome, email, senhaHash);

        await _usuarioRepository.AdicionarAsync(usuario);
        await _usuarioRepository.SalvarAlteracoesAsync();

        return _mapper.Map<UsuarioResponseDto>(usuario);
    }
}
