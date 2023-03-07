using Microsoft.AspNetCore.Identity;

namespace HCM.Auth.Data.Models;

public class ApplicationRole : IdentityRole
{
    public virtual ICollection<ApplicationUserRole<string>> Users { get; set; } = new List<ApplicationUserRole<string>>();
}