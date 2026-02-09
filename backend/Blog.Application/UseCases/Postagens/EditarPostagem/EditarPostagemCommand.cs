using Blog.Application.DTOs.Postagem;
using MediatR;

namespace Blog.Application.UseCases.Postagens.EditarPostagem;

public record EditarPostagemCommand(
    int Id,
    string Titulo,
    string Conteudo,
    int UsuarioId
) : IRequest<PostagemResponseDto>;
