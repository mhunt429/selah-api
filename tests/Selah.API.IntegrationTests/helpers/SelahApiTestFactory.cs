using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Selah.WebAPI;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Selah.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Selah.Domain.Data.Models.Authentication;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authentication.OAuth;

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

        /*
         * Generates a JWT for API endpoint testing
         * Doing this this way prevents having to inject the entire configuration into each test
         */
        public async Task<AuthenticationResponse> GenerateTestJwt(HttpClient client, AuthenticationRequest request)
        {
            var requestBody = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("api/v1/oauth/login", httpContent);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AuthenticationResponse>(content);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(T data, HttpClient client, string servicePath)
        {
            var requestBody = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            return await client.PostAsync(servicePath, httpContent);
        }
    }
}
