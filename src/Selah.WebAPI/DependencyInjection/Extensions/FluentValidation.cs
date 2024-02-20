using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Selah.Application.Commands.AppUser;
using Selah.Application.Commands.Transactions;
using Selah.Application.Queries.ApplicationUser;

namespace Selah.WebAPI.DependencyInjection.Extensions
{
    public static class FluentValidation
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommand.Validator>();
            services.AddScoped<IValidator<GetUserForLoginQuery>, GetUserForLoginQuery.Validator>();
            services.AddScoped<IValidator<CreateTransactionCommand>, CreateTransactionCommand.Validator>();
            return services;
        }
    }
}
