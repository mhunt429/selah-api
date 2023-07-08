using FluentAssertions;
using Selah.API.IntegrationTests.helpers;
using Selah.Application.Commands.AppUser;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Domain.Data.Models.Authentication;
using Selah.Infrastructure.Repository;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Selah.Application.Services;
using Selah.Application.Services.Interfaces;
using Xunit;
using Moq;

namespace Selah.API.IntegrationTests.ControllerTests
{
    public class DashboardControllerTests : IAsyncLifetime
    {
        private SelahApiTestFactory _testFactory;
        private readonly BaseRepository _baseRepository;
        private readonly AppUserRepository _userRepository;
        private readonly ISecurityService _securityService;
       private  Mock<ILogger<SecurityService>> _securityServiceLogger = new Mock<ILogger<SecurityService>>();
        private AppUser _testUser = new AppUser { };
        private HttpClient _testClient;
        private string jwt;

        public DashboardControllerTests()
        {
            _testFactory = new SelahApiTestFactory();
            _testClient = _testFactory.CreateClient();
            _baseRepository = DatabaseHelpers.CreateBaseRepository();
            _userRepository = new AppUserRepository(_baseRepository);
            
            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"JWT_SECRET", "vXb29wBjmENLKHwcHzAnkJa9tNfBnKhNwZesbVCJQ54PgnOKbjoirmyUgX8A6kK1IHJtMJ+G2tdSbewEohTJO7iQnNaPvUlHxBqmQFU7rGYdcY9xfQHh7+sZw4TVKcwhV89jIyybAR4MNGhtQoy5KfYmJwtMAdff94CSZO/d+0MFxjkikO1A+gXQI7tuHXl/dzQxGKfZC30PhkDzxB4ax9z0P6NmzgT6pg4tRiRfw8LHGafiI75+w5Fk5Ks4R1lp+stL0b+4CIVKnhIUIzLiQvDmkKjNTqeEQ7S1ABbVXGjz6Im4l3uZ2d/WdnJ0a1KT5hh+qX7Fk25FITFKK0qVuA=="},
                    {"JWT_ISSUER", "TestingIssuer"},
                    {"AWS_CONFIG__ACCESS_KEY", "Key"},
                    {"AWS_CONFIG__SECRET", "Secret"},
                    {"AWS_CONFIG__KMS_KEY", "KMS_KEY"},
                    {"HASH_ID_SALT", "Secret"}
                    // add any other configuration values here
                })
                .Build();

            _securityService = new SecurityService(_securityServiceLogger.Object, configurationBuilder);
        }

        [Fact]
        public async Task Unauthorized_RequestReturns401()
        {
            var response = await _testFactory.GetAsync<CreateUserCommand>(_testClient, $"api/v1/users/{_testUser.Id}/dashboard/summary");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task InvalidUserId_RequestReturns403()
        {
            _testClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var response = await _testFactory.GetAsync<CreateUserCommand>(_testClient, $"api/v1/users/{0}/dashboard/summary");
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task ValidTokenAndUserId_RequestReturns200()
        {
            _testClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var hashId = _testFactory.GetEncodedToken(_testUser.Id, _securityService);
            var response = await _testFactory.GetAsync<CreateUserCommand>(_testClient, $"api/v1/users/{hashId}/dashboard/summary");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public async Task InitializeAsync()
        {
            _testUser = await DatabaseHelpers.CreateUser(_userRepository);
            var jwtResponse = await _testFactory.GenerateTestJwt(_testClient, new AuthenticationRequest
            {
                EmailOrUsername = _testUser.Email,
                Password = _testUser.Password
            });
            jwt = jwtResponse.AccessToken;
        }

        public async Task DisposeAsync()
        {
            await DatabaseHelpers.DeleteTestUsers(_baseRepository, _testUser.Id);
            _testClient.Dispose();
            _testFactory.Dispose();
        }
    }
}
