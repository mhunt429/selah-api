using Xunit;
using FluentAssertions;
using Selah.API.IntegrationTests.helpers;
using Selah.Domain.Data.Models.Authentication;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Infrastructure.Repository;
using System.Net;

namespace Selah.API.IntegrationTests.ControllerTests
{
    public class AuthControllerTest : IAsyncLifetime
    {
        private SelahApiTestFactory _testFactory;
        private readonly BaseRepository _baseRepository;
        private readonly AppUserRepository _userRepository;


        private AppUser _testUser = new AppUser { };
        private HttpClient _testClient;

        public AuthControllerTest()
        {
            _testFactory = new SelahApiTestFactory();
            _testClient = _testFactory.CreateClient();
            _baseRepository = DatabaseHelpers.CreateBaseRepository();
            _userRepository = new AppUserRepository(_baseRepository);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_On_Invalid_Login()
        {
            // Arrange
            var command = new AuthenticationRequest { EmailOrUsername = "bad_user", Password = "BadP@ssword" };
            
            // Act
            var response = await _testFactory.PostAsync<AuthenticationRequest>(command, _testClient, "api/v1/oauth/login");
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_Return_Ok_On_Valid_Login()
        {
            // Arrange
            var command = new AuthenticationRequest { EmailOrUsername = _testUser.Email, Password = _testUser.Password };
          
            // Act
            var response = await _testFactory.PostAsync<AuthenticationRequest>(command, _testClient, "api/v1/oauth/login");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public async Task InitializeAsync()
        {
            _testUser = await DatabaseHelpers.CreateUser(_userRepository);
        }

        public async Task DisposeAsync()
        {
            await DatabaseHelpers.DeleteTestUsers(_baseRepository, _testUser.Id);
            _testClient.Dispose();
            _testFactory.Dispose();
        }
    }
}
