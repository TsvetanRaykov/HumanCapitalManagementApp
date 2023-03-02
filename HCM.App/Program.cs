using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(authenticationOptions =>
{
    authenticationOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    authenticationOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddOpenIdConnect(openIdConnectOptions =>
    {
        openIdConnectOptions.Authority = builder.Configuration["Authentication:Authority"];
        openIdConnectOptions.ClientId = builder.Configuration["Authentication:ClientId"];
        openIdConnectOptions.ClientSecret = builder.Configuration["Authentication:ClientSecret"];
        openIdConnectOptions.GetClaimsFromUserInfoEndpoint = true;
        openIdConnectOptions.ResponseType = "code";
        openIdConnectOptions.Scope.Add("https://human-capital-management.com/api");
        openIdConnectOptions.SaveTokens = true;
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
