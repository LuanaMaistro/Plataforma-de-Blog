using Blog.Domain.Exceptions;
using Blog.Domain.Interfaces;
using MediatR;

namespace Blog.Application.UseCases.Postagens.DeletarPostagem;

public class DeletarPostagemHandler : IRequestHandler<DeletarPostagemCommand, bool>
{
    private readonly IPostagemRepository _postagemRepository;

    public DeletarPostagemHandler(IPostagemRepository postagemRepository)
    {
        _postagemRepository = postagemRepository;
    }

    public async Task<bool> Handle(DeletarPostagemCommand request, CancellationToken cancellationToken)
    {
        var postagem = await _postagemRepository.ObterPorIdAsync(request.Id);

        if (postagem is null)
            throw new BusinessRuleException("Postagem nao encontrada");

        if (!postagem.PertenceAoUsuario(request.UsuarioId))
            throw new BusinessRuleException("Voce nao tem permissao para deletar esta postagem");

        await _postagemRepository.RemoverAsync(postagem);
        await _postagemRepository.SalvarAlteracoesAsync();

        return true;
    }
}
