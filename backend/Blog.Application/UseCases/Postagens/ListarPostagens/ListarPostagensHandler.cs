using AutoMapper;
using Blog.Application.DTOs.Postagem;
using Blog.Domain.Interfaces;
using MediatR;

namespace Blog.Application.UseCases.Postagens.ListarPostagens;

public class ListarPostagensHandler : IRequestHandler<ListarPostagensQuery, IEnumerable<PostagemResponseDto>>
{
    private readonly IPostagemRepository _postagemRepository;
    private readonly IMapper _mapper;

    public ListarPostagensHandler(IPostagemRepository postagemRepository, IMapper mapper)
    {
        _postagemRepository = postagemRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PostagemResponseDto>> Handle(ListarPostagensQuery request, CancellationToken cancellationToken)
    {
        var postagens = request.UsuarioId.HasValue
            ? await _postagemRepository.ListarPorUsuarioAsync(request.UsuarioId.Value)
            : await _postagemRepository.ListarTodasAsync();

        return _mapper.Map<IEnumerable<PostagemResponseDto>>(postagens);
    }
}
