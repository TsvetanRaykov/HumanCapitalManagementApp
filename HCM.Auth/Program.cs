using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using HCM.Auth.Data;
using HCM.Auth.Factories;
using HCM.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System.Reflection;
using HCM.Shared;
using IdentityModel;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>((provider, optionsBuilder) =>
{
    var connectionString = provider.GetRequiredService<IConfiguration>().GetConnectionString("Identity");
    optionsBuilder.UseNpgsql(connectionString, NpgSqlOptionsAction);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<ApplicationUser>()
    .AddConfigurationStore(configurationStoreOptions =>
    {
        configurationStoreOptions.ResolveDbContextOptions = ResolveDbContextOptions;
    })
    .AddOperationalStore(operationalStoreOptions =>
    {
        operationalStoreOptions.ResolveDbContextOptions = ResolveDbContextOptions;
    });

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

app.MapRazorPages();


if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();


    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    if (!await roleManager.RoleExistsAsync("admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("admin"));
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
        await configurationDbContext.ApiResources.AddAsync(new ApiResource
        {
            Name = "HCM.Api",
            DisplayName = "API",
            Scopes = new List<string> { HcmConstants.SupportedCustomOidcScopes.HcmApiScope }
        }.ToEntity());

        await configurationDbContext.SaveChangesAsync();
    }

    if (!await configurationDbContext.ApiScopes.AnyAsync())
    {
        await configurationDbContext.ApiScopes.AddAsync(new ApiScope
        {
            Name = HcmConstants.SupportedCustomOidcScopes.HcmApiScope,
            DisplayName = "API"
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
            AllowedScopes = new List<string> { HcmConstants.SupportedCustomOidcScopes.HcmApiScope },
            AllowedCorsOrigins = new List<string>
            {
                "https://api:7001",
                "https://localhost:7001"
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

app.Run();

static void NpgSqlOptionsAction(NpgsqlDbContextOptionsBuilder contextOptionsBuilder)
{
    var assembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
    contextOptionsBuilder.MigrationsAssembly(assembly);
}

static void ResolveDbContextOptions(IServiceProvider serviceProvider, DbContextOptionsBuilder dbContextOptionsBuilder)
{
    var connectionString = serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("IdentityServer");
    dbContextOptionsBuilder.UseNpgsql(connectionString, NpgSqlOptionsAction);
}