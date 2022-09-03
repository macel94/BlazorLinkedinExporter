using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorLinkedinExporter;
using Microsoft.Fast.Components.FluentUI;
using BlazorLinkedinExporter.Services;
using Blazored.LocalStorage;
using BlazorLinkedinExporter.Configuration;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var options = builder.Configuration.GetSection(LinkedinConfigurationOptions.ConfigurationKey);
builder.Services.Configure<LinkedinConfigurationOptions>(options);
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

//TODO
// AddHttpClient is an extension in Microsoft.Extensions.Http --> CLEANER WAY to implement httpclient
//builder.Services.AddHttpClient("WebAPI",
//        client => client.BaseAddress = new Uri("https://www.example.com/base"))
//    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

//builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
//    .CreateClient("WebAPI"));

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddFluentUIComponents();

//builder.Services.AddOptions();
//builder.Services.AddOidcAuthentication(options =>
//{
//    builder.Configuration.Bind("Local", options.ProviderOptions);
//    options.ProviderOptions.DefaultScopes.Add("r_emailaddress");
//    options.ProviderOptions.DefaultScopes.Add("r_liteprofile");
//});
builder.Services.AddLinkedinAuthenticationClient();
var app = builder.Build();

await app.RunAsync();
