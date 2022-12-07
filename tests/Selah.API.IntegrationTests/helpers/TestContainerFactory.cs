using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Selah.API.IntegrationTests.helpers
{
    public class TestContainerFactory : IAsyncLifetime
    {
        private readonly TestcontainerDatabase _testcontainers = new TestcontainersBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(new PostgreSqlTestcontainerConfiguration
        {
            Database = "postgres",
            Username = "postgres",
            Password = "postgres",
        })
        .WithEnvironment("JWT_SECRET", "Test_Secret")
        .WithEnvironment("JWT_ISSUER", "Selah_Test")
        .Build();
       

        public async Task DisposeAsync()
        {
            await _testcontainers.StopAsync();
        }

        public async Task InitializeAsync()
        {
            await _testcontainers.StartAsync();
            Environment.SetEnvironmentVariable("JWT_SECRET", "Test_Secret");
            Environment.SetEnvironmentVariable("JWT_ISSUER", "Selah_Test");
            Environment.SetEnvironmentVariable("DB_CONNECTION_STRING", "User ID=postgres;Password=postgres;Host=localhost;Port=9999;Database=postgres");
        }
    }
}
