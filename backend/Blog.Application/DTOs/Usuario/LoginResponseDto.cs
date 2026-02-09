namespace Blog.Application.DTOs.Usuario;

public record LoginResponseDto(
    int Id,
    string Nome,
    string Email,
    string Token
);
