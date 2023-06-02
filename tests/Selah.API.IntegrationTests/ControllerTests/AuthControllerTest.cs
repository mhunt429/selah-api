using System.Text;
using Xunit;
using FluentAssertions;
using Selah.API.IntegrationTests.helpers;
using Selah.Domain.Data.Models.Authentication;
using Newtonsoft.Json;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using Selah.Infrastructure;
using System.Net;

namespace Selah.API.IntegrationTests.ControllerTests
{
    public class AuthControllerTest : IAsyncLifetime
    {
        private SelahApiTestFactory _testFactory;
        private readonly BaseRepository _baseRepository;
        private readonly AppUserRepository _userRepository;
        private readonly Mock<ILogger<BaseRepository>> _loggerMock;

        private AppUser testUser = new AppUser { };
        private HttpClient _testClient;

        public AuthControllerTest()
        {
            var dbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                dbConnectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=65432;Database=postgres;Include Error Detail=true;";
            }
            _loggerMock = new Mock<ILogger<BaseRepository>>();

            _baseRepository = new BaseRepository(new NpgsqlConnectionFactory(dbConnectionString), _loggerMock.Object);
            _userRepository = new AppUserRepository(_baseRepository);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_On_Invalid_Login()
        {
            // Arrange
            var login = new AuthenticationRequest { EmailOrUsername = "bad_user", Password = "BadP@ssword" };
            var requestBody = JsonConvert.SerializeObject(login, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            // Act
            var response = await _testClient.PostAsync("api/v1/oauth/login", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_Return_Ok_On_Valid_Login()
        {
            // Arrange
            var login = new AuthenticationRequest { EmailOrUsername = testUser.Email, Password = testUser.Password };
            var requestBody = JsonConvert.SerializeObject(login, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            // Act
            var response = await _testClient.PostAsync("api/v1/oauth/login", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public async Task InitializeAsync()
        {
            _testFactory = new SelahApiTestFactory();
            _testClient = _testFactory.CreateClient();
            testUser = await DatabaseHelpers.CreateUser(_userRepository);
        }

        public async Task DisposeAsync()
        {
            await DatabaseHelpers.DeleteTestUsers(_baseRepository);
            _testClient.Dispose();
            _testFactory.Dispose();
        }
    }
}
