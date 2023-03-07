using AutoMapper;
using HCM.Auth.Data.Models;
using HCM.Shared.Data.DTO;

namespace HCM.Auth.Data.Mapping;

public class RoleResolver : IValueResolver<ApplicationUser, UserDto, string>
{
    public string Resolve(ApplicationUser source, UserDto destination, string destMember, ResolutionContext context)
    {
        if (source.Roles.Any()) 
            return source.Roles.First().Role.Name;
        return string.Empty;
    }
}