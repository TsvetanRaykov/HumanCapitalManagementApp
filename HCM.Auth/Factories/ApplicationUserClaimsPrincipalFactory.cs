using System.Security.Claims;
using HCM.Auth.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace HCM.Auth.Factories;

public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
{
    public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> optionsAccessor) 
        : base(userManager, optionsAccessor)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var claimsIdentity = await base.GenerateClaimsAsync(user);

        if (!string.IsNullOrWhiteSpace(user.GivenName))
        {
            claimsIdentity.AddClaim(new Claim(JwtClaimTypes.GivenName, user.GivenName));
        }

        if (!string.IsNullOrWhiteSpace(user.FamilyName))
        {
            claimsIdentity.AddClaim(new Claim(JwtClaimTypes.FamilyName, user.FamilyName));
        }

        return claimsIdentity;

    }
}