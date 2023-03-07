using AutoMapper;
using HCM.Shared.Data.DTO;

namespace HCM.App.Models.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserViewModel, UserWithPasswordDto>()
            .ForMember(dest=>dest.Password, opt=>opt.MapFrom(src=>src.Passwords!.Password));
        CreateMap<UserWithPasswordDto, UserViewModel>();

        CreateMap<UserViewModel, UserDto>();
        CreateMap<UserDto, UserViewModel>();
    }
}