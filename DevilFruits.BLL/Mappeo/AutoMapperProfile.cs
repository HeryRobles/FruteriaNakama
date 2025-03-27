using AutoMapper;
using DevilFruits.DTO.Models;
using DevilFruits.Model.Entities;

namespace DevilFruits.BLL.Mappeo
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dest => dest.Pass, opt => opt.Ignore( ));

            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(dest => dest.Pass, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Pass) ? null : BCrypt.Net.BCrypt.HashPassword(src.Pass)))
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Rol.ToLower()));

            CreateMap<Favorito, FavoritoDTO>().ReverseMap();

            CreateMap<Reseña, ResenaDTO>().ReverseMap();


        }

    }
}
