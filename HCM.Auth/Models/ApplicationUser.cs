using Microsoft.AspNetCore.Identity;

namespace HCM.Auth.Models;

public class ApplicationUser : IdentityUser
{
    public string? GivenName { get; set; }

    public string? FamilyName { get; set; }
}