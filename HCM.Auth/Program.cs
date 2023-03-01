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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    if (await userManager.FindByNameAsync("tsvetan.raykov") == null)
    {
        await userManager.CreateAsync(new ApplicationUser
        {
            UserName = "tsvetan.raykov",
            GivenName = "Tsvetan",
            FamilyName = "Raykov",
            Email = "tsvetan.raykov@gmail.com"
        }, "P@$$w0rd");
    }

    var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

    if (!await configurationDbContext.ApiResources.AnyAsync())
    {
        await configurationDbContext.ApiResources.AddAsync(new ApiResource
        {
            Name = Guid.NewGuid().ToString(),
            DisplayName = "API",
            Scopes = new List<string> { "https://human-capital-management.com/api" }
        }.ToEntity());

        await configurationDbContext.SaveChangesAsync();
    }


    if (!await configurationDbContext.ApiScopes.AnyAsync())
    {
        await configurationDbContext.ApiScopes.AddAsync(new ApiScope
        {
            Name = "https://human-capital-management.com/api",
            DisplayName = "API"
        }.ToEntity());

        await configurationDbContext.SaveChangesAsync();
    }

    if (!await configurationDbContext.Clients.AnyAsync())
    {
        await configurationDbContext.Clients.AddRangeAsync(new Client
        {
            ClientId = Guid.NewGuid().ToString(),
            ClientSecrets = new List<Secret> { new("AeSrYw9#6t".Sha512()) },
            ClientName = "Console Application",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes = new List<string> { "https://human-capital-management.com/api" },
            AllowedCorsOrigins = new List<string> { "https://api:7001" }
        }.ToEntity(), new Client
        {
            ClientId = Guid.NewGuid().ToString(),
            ClientSecrets = new List<Secret> { new("AeSrYw9#6t".Sha512()) },
            ClientName = "Web Application",
            AllowedGrantTypes = GrantTypes.Code,
            AllowedScopes = new List<string> { "https://human-capital-management.com/api", "profile", "openid", "email" },
            RedirectUris = new List<string> { "https://app:7002/signin-oidc" },
            PostLogoutRedirectUris = new List<string> { "https://app:7002/signout-callback-oidc" }
        }.ToEntity(), new Client
        {
            ClientId = Guid.NewGuid().ToString(),
            RequireClientSecret = false,
            ClientName = "Single Page Application",
            AllowedGrantTypes = GrantTypes.Code,
            AllowedScopes = new List<string> { "https://human-capital-management.com/api", "profile", "openid", "email" },
            AllowedCorsOrigins = new List<string> { "https://spa:7003" },
            RedirectUris = new List<string> { "https://spa:7003/authentication/login-callback" },
            PostLogoutRedirectUris = new List<string> { "https://spa:7003/authentication/logout-callback" }
        }.ToEntity());

        await configurationDbContext.SaveChangesAsync();
    }

    if (!await configurationDbContext.IdentityResources.AnyAsync())
    {
        await configurationDbContext.IdentityResources.AddRangeAsync(
            new IdentityResources.OpenId().ToEntity(),
            new IdentityResources.Profile().ToEntity(),
            new IdentityResources.Email().ToEntity());

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