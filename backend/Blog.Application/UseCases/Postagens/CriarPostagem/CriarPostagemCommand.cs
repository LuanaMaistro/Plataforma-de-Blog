using Blog.Application.DTOs.Postagem;
using MediatR;

namespace Blog.Application.UseCases.Postagens.CriarPostagem;

public record CriarPostagemCommand(
    string Titulo,
    string Conteudo,
    int UsuarioId
) : IRequest<PostagemResponseDto>;
