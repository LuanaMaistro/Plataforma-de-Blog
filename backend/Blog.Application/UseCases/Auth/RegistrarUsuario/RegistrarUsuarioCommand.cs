using Blog.Application.DTOs.Usuario;
using MediatR;

namespace Blog.Application.UseCases.Auth.RegistrarUsuario;

public record RegistrarUsuarioCommand(
    string Nome,
    string Email,
    string Senha
) : IRequest<UsuarioResponseDto>;
