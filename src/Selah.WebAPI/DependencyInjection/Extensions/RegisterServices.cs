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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IPlaidService, PlaidService>();
            services.AddScoped<IBankingService, BankingService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IInvestmentService, InvestmentService>();
            services.AddScoped<IAuthorizedAppService, AuthorizedAppService>();

            return services;
        }
    }
}
