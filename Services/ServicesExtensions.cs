using MACEL94.github.io.Services.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace MACEL94.github.io.Services
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddLinkedinAuthenticationClient(this IServiceCollection services)
        {
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            services.AddScoped<AuthStateProvider, AuthStateProvider>();
            return services;
        }
    }
}
