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
                {"JWT_SECRET", "vXb29wBjmENLKHwcHzAnkJa9tNfBnKhNwZesbVCJQ54PgnOKbjoirmyUgX8A6kK1IHJtMJ+G2tdSbewEohTJO7iQnNaPvUlHxBqmQFU7rGYdcY9xfQHh7+sZw4TVKcwhV89jIyybAR4MNGhtQoy5KfYmJwtMAdff94CSZO/d+0MFxjkikO1A+gXQI7tuHXl/dzQxGKfZC30PhkDzxB4ax9z0P6NmzgT6pg4tRiRfw8LHGafiI75+w5Fk5Ks4R1lp+stL0b+4CIVKnhIUIzLiQvDmkKjNTqeEQ7S1ABbVXGjz6Im4l3uZ2d/WdnJ0a1KT5hh+qX7Fk25FITFKK0qVuA=="},
                {"JWT_ISSUER", "TestingIssuer"}
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
