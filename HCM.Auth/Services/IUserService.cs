using HCM.Shared.Data.DTO;

namespace HCM.Auth.Services;

public interface IUserService
{
    Task<ICollection<UserDto>> GetUsersAsync();
    Task<UserDto?> GetByIdAsync(string id);
    Task<UserDto> DeleteUserAsync(string id);
    Task<UserDto> UpdateUserAsync(UserWithPasswordDto user);
    Task<UserDto> CreateUserAsync(UserWithPasswordDto user);

}