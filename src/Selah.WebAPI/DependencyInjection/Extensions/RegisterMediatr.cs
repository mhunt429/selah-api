using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Selah.Application.Queries.Analytics;
using Selah.Application.Queries.ApplicationUser;
using System.Reflection;

namespace Selah.WebAPI.DependencyInjection.Extensions
{
    public static class RegisterMediatr
    {
        public static void RegisterQueries(this IServiceCollection services)
        {
            services.AddMediatR(typeof(GetUserQuery).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(UserDashboardQuery).GetTypeInfo().Assembly);

        }
    }
}
