using Blog.Application.DTOs.Usuario;
using MediatR;

namespace Blog.Application.UseCases.Auth.AtualizarPerfil;

public record AtualizarPerfilCommand(
    int UsuarioId,
    string Nome,
    string? SenhaAtual,
    string? NovaSenha
) : IRequest<UsuarioResponseDto>;
