using Microsoft.AspNetCore.Identity;

namespace HCM.Auth.Data.Models;

public class ApplicationUser : IdentityUser
{
    public string? GivenName { get; set; }

    public string? FamilyName { get; set; }

    public virtual ICollection<ApplicationUserRole<string>> Roles { get; set; } = new List<ApplicationUserRole<string>>();
}