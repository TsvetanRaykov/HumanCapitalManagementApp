using Microsoft.AspNetCore.Identity;

namespace HCM.Auth.Data.Models;

public class ApplicationUserRole<TKey> : IdentityUserRole<TKey> where TKey : IEquatable<TKey>
{
    public virtual ApplicationUser User { get; set; }

    public virtual ApplicationRole Role { get; set; }
}