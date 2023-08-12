using Microsoft.Extensions.DependencyInjection;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.WebAPI.DependencyInjection.Extensions
{
    public static class RegisterRepositories
    {
        public static IServiceCollection RegisterDbRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IBankingRepository, BankingRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
