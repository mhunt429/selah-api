using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Selah.WebAPI;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Selah.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace Selah.API.IntegrationTests.helpers
{
    public class SelahApiTestFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var configurationBuilder = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
                {
                {"JwtSecret", "TestingSecret"},
                {"JwtIssuer", "TestingIssuer"}
                // add any other configuration values here
                })
            .Build();
            var server = builder
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseConfiguration(configurationBuilder);
            })
            .ConfigureServices(services =>
            {
                var dbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
                //Github actions sets the field above so for local integration tests, just use the value found in the docker compose settings
                if (string.IsNullOrEmpty(dbConnectionString))
                {
                    dbConnectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=65432;Database=postgres";
                }
                services.RemoveAll(typeof(IDbConnectionFactory));
                services.AddSingleton<IDbConnectionFactory>(_ =>
                new NpgsqlConnectionFactory(dbConnectionString));
            });
            return base.CreateHost(server);
        }
    }
}
