using AutoMapper;
using Blog.Application.DTOs.Postagem;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using MediatR;

namespace Blog.Application.UseCases.Postagens.CriarPostagem;

public class CriarPostagemHandler : IRequestHandler<CriarPostagemCommand, PostagemResponseDto>
{
    private readonly IPostagemRepository _postagemRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMapper _mapper;

    public CriarPostagemHandler(
        IPostagemRepository postagemRepository,
        IUsuarioRepository usuarioRepository,
        IMapper mapper)
    {
        _postagemRepository = postagemRepository;
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
    }

    public async Task<PostagemResponseDto> Handle(CriarPostagemCommand request, CancellationToken cancellationToken)
    {
        var postagem = new Postagem(request.Titulo, request.Conteudo, request.UsuarioId);

        await _postagemRepository.AdicionarAsync(postagem);
        await _postagemRepository.SalvarAlteracoesAsync();

        var usuario = await _usuarioRepository.ObterPorIdAsync(request.UsuarioId);

        return new PostagemResponseDto(
            postagem.Id,
            postagem.Titulo,
            postagem.Conteudo,
            postagem.DataCriacao,
            postagem.DataAtualizacao,
            new AutorDto(usuario!.Id, usuario.Nome)
        );
    }
}
