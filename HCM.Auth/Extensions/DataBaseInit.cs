namespace HCM.Auth.Extensions;
using Data;
using Data.Models;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;

public static class DataBaseInit
{
    public static async Task DataBaseSeedAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
        await scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.MigrateAsync();
        await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();


        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[] { "admin", "user" })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = role });
            }
        }

        if (await userManager.FindByNameAsync("tsvetan.raykov") == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "tsvetan.raykov",
                GivenName = "Tsvetan",
                FamilyName = "Raykov",
                Email = "tsvetan.raykov@gmail.com"
            };

            await userManager.CreateAsync(admin, "P@$$w0rd");
            await userManager.AddToRoleAsync(admin, "admin");
        }

        var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

        if (!await configurationDbContext.ApiResources.AnyAsync())
        {
            await configurationDbContext.ApiResources.AddRangeAsync(new ApiResource
            {
                Name = "HCM.Api",
                DisplayName = "API",
                Scopes = new List<string> { HcmConstants.SupportedCustomOidcScopes.HcmApiScope }
            }.ToEntity(), new ApiResource
            {
                Name = "HCM.Ums",
                DisplayName = "UMS",
                Scopes = new List<string> { HcmConstants.SupportedCustomOidcScopes.HcmUmsScope }
            }.ToEntity());

            await configurationDbContext.SaveChangesAsync();
        }

        if (!await configurationDbContext.ApiScopes.AnyAsync())
        {
            await configurationDbContext.ApiScopes.AddRangeAsync(new ApiScope
            {
                Name = HcmConstants.SupportedCustomOidcScopes.HcmApiScope,
                DisplayName = "API"
            }.ToEntity(), new ApiScope
            {
                Name = HcmConstants.SupportedCustomOidcScopes.HcmUmsScope,
                DisplayName = "UMS"
            }.ToEntity());

            await configurationDbContext.SaveChangesAsync();
        }

        if (!await configurationDbContext.Clients.AnyAsync())
        {
            await configurationDbContext.Clients.AddRangeAsync(new Client
            {
                ClientId = "Human.Capital.Management.App",
                RequireClientSecret = false,
                ClientName = "Human Capital Management App",
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = new List<string>
            {
                HcmConstants.SupportedCustomOidcScopes.HcmApiScope,
                HcmConstants.SupportedCustomOidcScopes.HcmUmsScope,
                "profile",
                "openid",
                "email",
                "roles"
            },
                AllowedCorsOrigins = new List<string>
            {
                "https://singlepageapplication:7003",
                "https://localhost:7003",
                "https://localhost:7103"
            },
                RedirectUris = new List<string> {
                "https://singlepageapplication:7003/authentication/login-callback",
                "https://localhost:7003/authentication/login-callback",
                "https://localhost:7103/authentication/login-callback"
            },
                PostLogoutRedirectUris = new List<string>
            {
                "https://singlepageapplication:7003/authentication/logout-callback",
                "https://localhost:7003/authentication/logout-callback",
                "https://localhost:7103/authentication/logout-callback",
            }
            }.ToEntity(), new Client
            {
                ClientId = "Api.Test",
                ClientSecrets = new List<Secret> { new("t7687kH3Hp".Sha512()) },
                ClientName = "Api Test Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = new List<string>
                {
                    HcmConstants.SupportedCustomOidcScopes.HcmApiScope,
                    HcmConstants.SupportedCustomOidcScopes.HcmUmsScope
                },
                AllowedCorsOrigins = new List<string>
            {
                "https://api:7001",
                "https://localhost:7001",
                "https://localhost:7139"
            }
            }.ToEntity());

            await configurationDbContext.SaveChangesAsync();
        }

        if (!await configurationDbContext.IdentityResources.AnyAsync())
        {
            await configurationDbContext.IdentityResources.AddRangeAsync(
                new IdentityResources.OpenId().ToEntity(),
                new IdentityResources.Profile().ToEntity(),
                new IdentityResources.Email().ToEntity(),
                new IdentityResource
                {
                    Name = "roles",
                    DisplayName = "Roles",
                    UserClaims = { JwtClaimTypes.Role }
                }.ToEntity());

            await configurationDbContext.SaveChangesAsync();
        }
    }
}