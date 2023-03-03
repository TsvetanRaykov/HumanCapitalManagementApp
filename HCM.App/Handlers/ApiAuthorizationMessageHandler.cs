using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace HCM.App.Handlers;

public class ApiAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public ApiAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation) : base(provider,
        navigation)
    {
        ConfigureHandler(new List<string> { "https://localhost:7001" }, new List<string> { "https://human-capital-management.com/api" });
    }
}
