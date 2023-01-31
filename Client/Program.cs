using HotelFinal.Client;
using HotelFinal.Client.Services;
using HotelFinal.Client.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OpenTelemetry.Resources;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOpenTelemetryTracing(b =>
{
    b.SetResourceBuilder(
        ResourceBuilder.CreateDefault().AddService(builder.HostEnvironment))
    .AddAspNetCorInstrumentation()
    .AddHttpClientInstrumentation()
    .AddOtlpExporter(opts => { opts.Endpoint = new Uri("https://localhost:4137"); });
});

builder.Services.AddHttpClient("ServerAPI",
      client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddHttpClient<PublicClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
  .CreateClient("ServerAPI"));

builder.Services.AddSyncfusionBlazor();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzQ5NjIzQDMyMzAyZTMzMmUzMGUxMDZ6ZlZRbzlKcDlqZmZEMkh0NExETnAyMHg4S216bkozb3lrcmE1Ym89");

builder.Services.AddScoped<HotelService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);
}).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

await builder.Build().RunAsync();
