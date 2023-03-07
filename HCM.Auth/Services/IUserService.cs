using HCM.Shared.Data.DTO;

namespace HCM.Auth.Services;

public interface IUserService
{
    Task<ICollection<UserDto>> GetUsersAsync();
    Task<UserDto?> GetByIdAsync(string id);
    Task<UserDto> DeleteUserAsync(string id);
    Task<UserDto> UpdateUserAsync(UserDto user);
    Task<UserDto> CreateUserAsync(UserDto user, string password);
    Task<UserDto> SetPasswordAsync(string? userId, string password);
}