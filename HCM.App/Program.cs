using HCM.App;
using HCM.App.Handlers;
using HCM.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Api", httpClient =>
    {
        httpClient.BaseAddress = new Uri("https://api:7001");
    })
    .AddHttpMessageHandler<ApiAuthorizationMessageHandler>();

builder.Services.AddHttpClient("Ums", httpClient =>
    {
        httpClient.BaseAddress = new Uri("https://identityserver:7000");
    })
    .AddHttpMessageHandler<UmsAuthorizationMessageHandler>();

builder.Services.AddOidcAuthentication(remoteAuthenticationOptions =>
{
    remoteAuthenticationOptions.ProviderOptions.Authority = builder.Configuration["Authentication:Authority"];
    remoteAuthenticationOptions.ProviderOptions.ClientId = builder.Configuration["Authentication:ClientId"];
    remoteAuthenticationOptions.ProviderOptions.ResponseType = "code";
    remoteAuthenticationOptions.ProviderOptions.DefaultScopes.Add(HcmConstants.SupportedCustomOidcScopes.HcmApiScope);
    remoteAuthenticationOptions.ProviderOptions.DefaultScopes.Add(HcmConstants.SupportedCustomOidcScopes.HcmUmsScope);
    remoteAuthenticationOptions.ProviderOptions.DefaultScopes.Add("email");
    remoteAuthenticationOptions.ProviderOptions.DefaultScopes.Add("roles");
    remoteAuthenticationOptions.UserOptions.RoleClaim = "role";
});

builder.Services.AddScoped<ApiAuthorizationMessageHandler>();
builder.Services.AddScoped<UmsAuthorizationMessageHandler>();

await builder.Build().RunAsync();
