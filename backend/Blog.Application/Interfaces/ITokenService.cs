using Blog.Domain.Entities;

namespace Blog.Application.Interfaces;

public interface ITokenService
{
    string GerarToken(Usuario usuario);
}
