namespace Blog.Application.DTOs.Usuario;

public record UsuarioResponseDto(
    int Id,
    string Nome,
    string Email,
    DateTime DataCriacao
);
