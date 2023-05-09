using Microsoft.Extensions.DependencyInjection;
using Selah.Domain.Data.SchemaMappings;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.WebAPI.DependencyInjection.Extensions
{
    public static class RegisterRepositories
    {
        public static IServiceCollection RegisterDbRepositories(this IServiceCollection services)
        {
            //First register the schema mappings from sql column names to our object properties
            SelahMappings.RegisterMaps();

            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IBankingRepository, BankingRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
