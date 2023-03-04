using HCM.Api.Data;
using HCM.Api.Data.Repositories;
using HCM.Shared;
using HCM.Shared.Data.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApiDbContext>((serviceProvider, dbContextOptionsBuilder) =>
{
    var connectionString = serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("HcmDb");
    var migrationsAssembly = typeof(ApiDbContext).GetTypeInfo().Assembly.GetName().Name;

    dbContextOptionsBuilder.UseNpgsql(connectionString, optionsBuilder =>
    {
        optionsBuilder.MigrationsAssembly(migrationsAssembly);
    });
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

builder.Services.AddAuthorization(authorizationOptions =>
{
    authorizationOptions.AddPolicy("ApiScope", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser()
            .RequireClaim("scope", HcmConstants.SupportedCustomOidcScopes.HcmApiScope);
    });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddDefaultPolicy(corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HCM Api",
        Version = "v1"
    });

    swaggerGenOptions.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri($"{builder.Configuration["Authentication:Authority"]}/connect/token"),
                Scopes = { { HcmConstants.SupportedCustomOidcScopes.HcmApiScope, "API" } }
            }
        }
    });

    swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new List<string>{ HcmConstants.SupportedCustomOidcScopes.HcmApiScope }
        }
    });
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<ApiDbContext>().Database.MigrateAsync();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
