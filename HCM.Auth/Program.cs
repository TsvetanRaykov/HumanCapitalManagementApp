using HCM.Auth.Data;
using HCM.Auth.Data.Mapping;
using HCM.Auth.Data.Models;
using HCM.Auth.Extensions;
using HCM.Auth.Factories;
using HCM.Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(jwtBearerOptions =>
    {
        jwtBearerOptions.Authority = builder.Configuration["Authentication:Authority"];
        jwtBearerOptions.Audience = builder.Configuration["Authentication:Audience"];

        jwtBearerOptions.TokenValidationParameters.ValidateAudience = true;
        jwtBearerOptions.TokenValidationParameters.ValidateIssuer = true;
        jwtBearerOptions.TokenValidationParameters.ValidateIssuerSigningKey = true;

    });

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<RoleResolver>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();


if (app.Environment.IsDevelopment())
{
    await app.DataBaseSeedAsync();
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