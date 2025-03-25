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

            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dest => dest.Pass, opt => opt.Ignore());

            CreateMap<UsuarioDTO, Usuario>();

            CreateMap<Favorito, FavoritoDTO>();
            CreateMap<FavoritoDTO, Favorito>();

            CreateMap<Reseña, ResenaDTO>();
            CreateMap<ResenaDTO, Reseña>();


        }

    }
}
