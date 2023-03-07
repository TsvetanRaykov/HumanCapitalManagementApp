using AutoMapper;
using HCM.Auth.Data.Models;
using HCM.Shared.Data.DTO;

namespace HCM.Auth.Data.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom<RoleResolver>());
        CreateMap<UserDto, ApplicationUser>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}