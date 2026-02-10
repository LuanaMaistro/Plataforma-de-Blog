namespace Blog.Application.DTOs.Usuario;

public record AtualizarPerfilDto(
    string Nome,
    string? SenhaAtual,
    string? NovaSenha
);
