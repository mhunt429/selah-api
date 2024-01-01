using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Selah.WebAPI.DependencyInjection;

public static class HttpClientFactory
{
    public static IServiceCollection RegisterHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        /*
        services.AddHttpClient<IPlaidService, PlaidService>(client =>
        {
            client.BaseAddress = new Uri($"https://{configuration["PlaidEnv"]}.plaid.com");
        });
*/
        return services;
    }
}