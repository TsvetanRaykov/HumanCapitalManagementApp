using HCM.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace HCM.App.Handlers;

public class UmsAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public UmsAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation) : base(provider,
        navigation)
    {
        ConfigureHandler(new List<string> { "https://identityserver:7000" }, new List<string> { HcmConstants.SupportedCustomOidcScopes.HcmUmsScope });
    }
}