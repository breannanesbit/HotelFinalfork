using HotelFinal.Client;
using HotelFinal.Client.Services;
using HotelFinal.Client.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddHttpClient("ServerAPI",
//      client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
//    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
//  .CreateClient("ServerAPI"));

builder.Services.AddHttpClient();

builder.Services.AddScoped<HotelService>();

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);
}).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>(); ;

await builder.Build().RunAsync();
