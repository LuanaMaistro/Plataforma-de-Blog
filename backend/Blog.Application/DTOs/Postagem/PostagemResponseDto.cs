namespace Blog.Application.DTOs.Postagem;

public record PostagemResponseDto(
    int Id,
    string Titulo,
    string Conteudo,
    DateTime DataCriacao,
    DateTime? DataAtualizacao,
    AutorDto Autor
);

public record AutorDto(
    int Id,
    string Nome
);
