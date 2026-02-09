using Blog.Application.DTOs.Postagem;
using MediatR;

namespace Blog.Application.UseCases.Postagens.ObterPostagem;

public record ObterPostagemQuery(int Id) : IRequest<PostagemResponseDto?>;
