using HCM.Auth.Data.Models;
using HCM.Auth.Services;
using HCM.Shared.Data.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HCM.Auth.Controllers
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UsersController(IUserService userService, RoleManager<ApplicationRole> roleManager)
        {
            _userService = userService;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id.ToString());
            if (user == null) return NoContent();
            return Ok(user);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles
                .Select(r => r.Name)
                .ToArrayAsync();

            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserWithPasswordDto user)
        {
            try
            {
                var createResult = await _userService.CreateUserAsync(user, user.Password);
                return Ok(createResult);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto user)
        {
            try
            {
                var updateResult = await _userService.UpdateUserAsync(user);
                return Ok(updateResult);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("password")]
        public async Task<IActionResult> SetUserPassword(PasswordDto passwordDto)
        {
            try
            {
                var userDto = await _userService.SetPasswordAsync(passwordDto.UserId, passwordDto.Password);
                return Ok(userDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var deleteResult = await _userService.DeleteUserAsync(id.ToString());
                return Ok(deleteResult);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
