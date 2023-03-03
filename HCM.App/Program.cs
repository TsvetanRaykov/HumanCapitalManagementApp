using HCM.App;
using HCM.App.Handlers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Api", httpClient =>
    {
        httpClient.BaseAddress = new Uri("https://localhost:7001");
    })
    .AddHttpMessageHandler<ApiAuthorizationMessageHandler>();

builder.Services.AddOidcAuthentication(remoteAuthenticationOptions =>
{
    remoteAuthenticationOptions.ProviderOptions.Authority = builder.Configuration["Authentication:Authority"];
    remoteAuthenticationOptions.ProviderOptions.ClientId = builder.Configuration["Authentication:ClientId"];
    remoteAuthenticationOptions.ProviderOptions.ResponseType = "code";
    remoteAuthenticationOptions.ProviderOptions.DefaultScopes.Add("https://human-capital-management.com/api");
    remoteAuthenticationOptions.ProviderOptions.DefaultScopes.Add("email");
});

builder.Services.AddScoped<ApiAuthorizationMessageHandler>();

await builder.Build().RunAsync();
