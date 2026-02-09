using AutoMapper;
using Blog.Application.DTOs.Postagem;
using Blog.Domain.Exceptions;
using Blog.Domain.Interfaces;
using MediatR;

namespace Blog.Application.UseCases.Postagens.EditarPostagem;

public class EditarPostagemHandler : IRequestHandler<EditarPostagemCommand, PostagemResponseDto>
{
    private readonly IPostagemRepository _postagemRepository;
    private readonly IMapper _mapper;

    public EditarPostagemHandler(IPostagemRepository postagemRepository, IMapper mapper)
    {
        _postagemRepository = postagemRepository;
        _mapper = mapper;
    }

    public async Task<PostagemResponseDto> Handle(EditarPostagemCommand request, CancellationToken cancellationToken)
    {
        var postagem = await _postagemRepository.ObterPorIdAsync(request.Id);

        if (postagem is null)
            throw new BusinessRuleException("Postagem nao encontrada");

        if (!postagem.PertenceAoUsuario(request.UsuarioId))
            throw new BusinessRuleException("Voce nao tem permissao para editar esta postagem");

        postagem.Atualizar(request.Titulo, request.Conteudo);
        await _postagemRepository.SalvarAlteracoesAsync();

        return _mapper.Map<PostagemResponseDto>(postagem);
    }
}
