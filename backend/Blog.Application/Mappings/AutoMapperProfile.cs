using AutoMapper;
using Blog.Application.DTOs.Postagem;
using Blog.Application.DTOs.Usuario;
using Blog.Domain.Entities;

namespace Blog.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Usuario
        CreateMap<Usuario, UsuarioResponseDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Valor));

        // Postagem
        CreateMap<Postagem, PostagemResponseDto>()
            .ForMember(dest => dest.Autor, opt => opt.MapFrom(src => src.Usuario));

        CreateMap<Usuario, AutorDto>();
    }
}
