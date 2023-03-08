using HCM.Shared.Data.DTO;

namespace HCM.App.Models.Mapping;

public static class UserMappingProfile
{
    public static UserViewModel ToUserViewModel(this UserWithPasswordDto dto)
    {
        return new UserViewModel
        {
            Id = dto.Id,
            UserName = dto.UserName,
            Role = dto.Role,
            Email = dto.Email,
            FamilyName = dto.FamilyName,
            GivenName = dto.GivenName,
            PhoneNumber = dto.PhoneNumber,
            Passwords = new PasswordViewModel
            {
                Password = dto.Password
            }
        };
    }

    public static UserWithPasswordDto ToUserWithPasswordDto(this UserViewModel vm)
    {
        return new UserWithPasswordDto
        {
            Id = vm.Id,
            UserName = vm.UserName,
            Role = vm.Role!,
            Email = vm.Email,
            FamilyName = vm.FamilyName,
            GivenName = vm.GivenName,
            PhoneNumber = vm.PhoneNumber,
            Password = vm.Passwords?.Password
        };
    }

    public static UserViewModel ToUserViewModel(this UserDto dto)
    {
        return new UserViewModel
        {
            Id = dto.Id,
            UserName = dto.UserName,
            Role = dto.Role!,
            Email = dto.Email,
            FamilyName = dto.FamilyName,
            GivenName = dto.GivenName,
            PhoneNumber = dto.PhoneNumber,
        };
    }

    public static UserDto ToUserDto(this UserViewModel vm)
    {
        return new UserDto
        {
            Id = vm.Id,
            UserName = vm.UserName,
            Role = vm.Role!,
            Email = vm.Email,
            FamilyName = vm.FamilyName,
            GivenName = vm.GivenName,
            PhoneNumber = vm.PhoneNumber,
        };
    }

}