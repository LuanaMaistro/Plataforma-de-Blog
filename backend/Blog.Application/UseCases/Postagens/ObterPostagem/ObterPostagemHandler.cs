using AutoMapper;
using Blog.Application.DTOs.Postagem;
using Blog.Domain.Interfaces;
using MediatR;

namespace Blog.Application.UseCases.Postagens.ObterPostagem;

public class ObterPostagemHandler : IRequestHandler<ObterPostagemQuery, PostagemResponseDto?>
{
    private readonly IPostagemRepository _postagemRepository;
    private readonly IMapper _mapper;

    public ObterPostagemHandler(IPostagemRepository postagemRepository, IMapper mapper)
    {
        _postagemRepository = postagemRepository;
        _mapper = mapper;
    }

    public async Task<PostagemResponseDto?> Handle(ObterPostagemQuery request, CancellationToken cancellationToken)
    {
        var postagem = await _postagemRepository.ObterPorIdAsync(request.Id);

        if (postagem is null)
            return null;

        return _mapper.Map<PostagemResponseDto>(postagem);
    }
}
