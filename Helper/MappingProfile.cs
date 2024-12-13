using APIServerSmartHome.DTOs;
using APIServerSmartHome.Entities;
using AutoMapper;

namespace APIServerSmartHome.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            CreateMap<ProfileDTO, User>()
            .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Phonenumber, opt => opt.MapFrom(src => src.Phonenumber))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
        }
    }
}
