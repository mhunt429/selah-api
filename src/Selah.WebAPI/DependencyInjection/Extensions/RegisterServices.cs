using Microsoft.Extensions.DependencyInjection;
using Selah.Application.Services;
using Selah.Application.Services.Interfaces;
using Selah.Infrastructure.Services;
using Selah.Infrastructure.Services.Validators;

namespace Selah.WebAPI.DependencyInjection.Extensions
{
    public static class RegisterServices
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITransactionValidatorService, TransactionValidatorService>();
            return services;
        }
    }
}
