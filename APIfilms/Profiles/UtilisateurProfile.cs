using AutoMapper;
using APIfilms.Models.EntityFramework;
using APIfilms.Models.DTO;

namespace APIfilms.Profiles
{
    public class UtilisateurProfile : Profile
    {
        public UtilisateurProfile()
        {
            CreateMap<Utilisateur, UtilisateurDto>();

            CreateMap<Utilisateur, UtilisateurDetailDto>()
                .ForMember(dest => dest.RoleDescription, opt => opt.MapFrom(src => "Standard User"));
        }
    }
}
