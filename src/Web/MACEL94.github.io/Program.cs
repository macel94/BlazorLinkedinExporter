using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MACEL94.github.io;
using Microsoft.Fast.Components.FluentUI;
using MACEL94.github.io.Services;
using Blazored.LocalStorage;
using MACEL94.github.io.Configuration;

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
