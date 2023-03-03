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

builder.Services.AddOidcAuthentication(remoteAuthenticationOptions =>
{
    remoteAuthenticationOptions.ProviderOptions.Authority = builder.Configuration["Authentication:Authority"];
    remoteAuthenticationOptions.ProviderOptions.ClientId = builder.Configuration["Authentication:ClientId"];
    remoteAuthenticationOptions.ProviderOptions.ResponseType = "code";
    remoteAuthenticationOptions.ProviderOptions.DefaultScopes.Add(HcmConstants.SupportedCustomOidcScopes.HcmApiScope);
    remoteAuthenticationOptions.ProviderOptions.DefaultScopes.Add("email");
    remoteAuthenticationOptions.ProviderOptions.DefaultScopes.Add("roles");
});

builder.Services.AddScoped<ApiAuthorizationMessageHandler>();

await builder.Build().RunAsync();
