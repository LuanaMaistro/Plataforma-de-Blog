using Blog.Application.DTOs.Postagem;
using MediatR;

namespace Blog.Application.UseCases.Postagens.ListarPostagens;

public record ListarPostagensQuery(
    int? UsuarioId = null
) : IRequest<IEnumerable<PostagemResponseDto>>;
