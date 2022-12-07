using AutoMapper;
using Selah.Domain.Data.Models.ApplicationUser;

namespace Selah.Application.Mappings.AppUser
{
    public class AppUserProfile : Profile
    {
        public AppUserProfile()
        {
            CreateMap<Domain.Data.Models.ApplicationUser.AppUser, UserViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                   .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                     .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                     .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                      .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated));

        }
    }
}
