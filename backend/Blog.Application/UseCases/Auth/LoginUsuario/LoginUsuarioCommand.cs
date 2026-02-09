using Blog.Application.DTOs.Usuario;
using MediatR;

namespace Blog.Application.UseCases.Auth.LoginUsuario;

public record LoginUsuarioCommand(
    string Email,
    string Senha
) : IRequest<LoginResponseDto>;
