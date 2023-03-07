using AutoMapper;
using HCM.Auth.Data.Models;
using HCM.Shared.Data.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HCM.Auth.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ICollection<UserDto>> GetUsersAsync()
    {
        var users = await UsersWithRoles.ToArrayAsync();
        return _mapper.Map<UserDto[]>(users);
    }

    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var dbUser = await UsersWithRoles
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();

        return _mapper.Map<UserDto>(dbUser);
    }

    public async Task<UserDto> DeleteUserAsync(string id)
    {
        var dbUser = await UsersWithRoles
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();

        var userDto = _mapper.Map<UserDto>(dbUser);

        if (dbUser == null)
            throw new Exception($"User with Id {id} not found");

        var roles = await _userManager.GetRolesAsync(dbUser);

        await _userManager.RemoveFromRolesAsync(dbUser, roles);

        var result = await _userManager.DeleteAsync(dbUser);

        EnsureSuccess(result);
        return userDto;
    }

    public async Task<UserDto> UpdateUserAsync(UserDto user)
    {
        var dbUser = await UsersWithRoles
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (dbUser == null)
            throw new Exception($"User {user.UserName} not found");


        await EnsureRolesAsync(dbUser, user.Role);

        _mapper.Map(user, dbUser);
        var result = await _userManager.UpdateAsync(dbUser);

        EnsureSuccess(result);
        return _mapper.Map(dbUser, user);
    }

    public async Task<UserDto> CreateUserAsync(UserDto user, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new Exception("Password is required.");

        var dbUser = _mapper.Map(user, new ApplicationUser());

        var result = await _userManager.CreateAsync(dbUser, password);
        EnsureSuccess(result);

        try
        {
            await EnsureRolesAsync(dbUser, user.Role);
        }
        catch (Exception)
        {
            await _userManager.DeleteAsync(dbUser);
            throw;
        }

        return _mapper.Map<UserDto>(dbUser);
    }

    public async Task<UserDto> SetPasswordAsync(string? userId, string password)
    {
        var dbUser = await _userManager.FindByIdAsync(userId);

        if (dbUser == null)
            throw new Exception($"User with Id {userId} not found");

        EnsureSuccess(await _userManager.RemovePasswordAsync(dbUser));
        EnsureSuccess(await _userManager.AddPasswordAsync(dbUser, password));

        return _mapper.Map<UserDto>(dbUser);
    }

    private IQueryable<ApplicationUser> UsersWithRoles => _userManager.Users
        .Include(u => u.Roles)
        .ThenInclude(r => r.Role);

    private async Task EnsureRolesAsync(ApplicationUser dbUser, string roleName)
    {
        var currentRoles = await _userManager.GetRolesAsync(dbUser);

        if (!currentRoles.Contains(roleName))
        {
            await _userManager.RemoveFromRolesAsync(dbUser, currentRoles);
            await _userManager.AddToRoleAsync(dbUser, roleName);
        }
    }

    private void EnsureSuccess(IdentityResult result)
    {
        if (result.Succeeded) return;
        throw new Exception(result.Errors.FirstOrDefault()?.Description);
    }
}