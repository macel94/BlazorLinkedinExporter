using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MACEL94.github.io;
using Microsoft.Fast.Components.FluentUI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddFluentUIComponents();

//builder.Services.AddOptions();
//builder.Services.AddOidcAuthentication(options =>
//{
//    builder.Configuration.Bind("Local", options.ProviderOptions);
//    options.ProviderOptions.DefaultScopes.Add("r_emailaddress");
//    options.ProviderOptions.DefaultScopes.Add("r_liteprofile");
//});
builder.Services.AddSingleton(sp =>
{
    var dedicatedHttpClient = new HttpClient();
    return new LinkedinClient(dedicatedHttpClient);
});

await builder.Build().RunAsync();
