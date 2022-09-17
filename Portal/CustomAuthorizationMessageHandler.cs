using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Portal;

public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public static readonly string[] ApiScopes = {           
        " https://RetailManage.onmicrosoft.com/RetailManagerApi/data.view",
        "https://RetailManage.onmicrosoft.com/RetailManagerApi/data.write"
            
    };
    public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation) : base(provider, navigation)
    {
        ConfigureHandler(
            authorizedUrls: new[] { "https://localhost:7145/" },
            scopes: ApiScopes);
    }
}