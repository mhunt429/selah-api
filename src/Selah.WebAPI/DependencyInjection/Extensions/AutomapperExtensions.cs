using Microsoft.Extensions.DependencyInjection;
using Selah.Application.Mappings.AppUser;

namespace Selah.WebAPI.DependencyInjection.Extensions
{
    public static class AutomapperExtensions
    {
        public static IServiceCollection RegisterMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AppUserProfile));
            return services;
        }
    }
}
