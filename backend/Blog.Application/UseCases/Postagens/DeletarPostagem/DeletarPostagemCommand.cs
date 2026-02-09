using MediatR;

namespace Blog.Application.UseCases.Postagens.DeletarPostagem;

public record DeletarPostagemCommand(
    int Id,
    int UsuarioId
) : IRequest<bool>;
