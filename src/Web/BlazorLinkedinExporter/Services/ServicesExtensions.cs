using BlazorLinkedinExporter.Services.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorLinkedinExporter.Services
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddLinkedinAuthenticationStateProvider(this IServiceCollection services)
        {
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            services.AddScoped<AuthStateProvider, AuthStateProvider>();
            return services;
        }
    }
}
