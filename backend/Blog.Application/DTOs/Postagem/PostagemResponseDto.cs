namespace Blog.Application.DTOs.Postagem;

public class PostagemResponseDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public AutorDto Autor { get; set; } = null!;
}

public class AutorDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}
