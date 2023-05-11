using Microsoft.Extensions.DependencyInjection;
using Selah.Application.Services;
using Selah.Application.Services.Interfaces;

namespace Selah.WebAPI.DependencyInjection.Extensions
{
    public static class RegisterServices
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
         
            services.AddScoped<ISecurityService, SecurityService>();
            return services;
        }
    }
}
