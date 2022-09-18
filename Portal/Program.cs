using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Portal;
using Protal.Library;
using RMWPFUserInterface.Library.Helpers;
using RMWPFUserInterface.Library.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<CustomAuthorizationMessageHandler>();
builder.Services.AddHttpClient(ApiEndPoints.ApiClientName, client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
builder.Services.AddBlazoredLocalStorage();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(ApiEndPoints.ApiClientName));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add(" https://RetailManage.onmicrosoft.com/RetailManagerApi/data.view");
    options.ProviderOptions.DefaultAccessTokenScopes.Add(" https://RetailManage.onmicrosoft.com/RetailManagerApi/data.write");
});

builder.Services.AddSingleton<IApiHelper, ApiHelper>();
builder.Services.AddSingleton<ILoggedInUserModel, LoggedInUserModel>();
builder.Services.AddTransient<IProductEndPoint, ProductEndPoint>();
builder.Services.AddTransient<ISaleEndPoint, SaleEndPoint>();

await builder.Build().RunAsync();