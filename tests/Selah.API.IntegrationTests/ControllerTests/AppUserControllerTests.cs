using FluentAssertions;
using Selah.API.IntegrationTests.helpers;
using Selah.Application.Commands.AppUser;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Domain.Data.Models.Authentication;
using Selah.Infrastructure.Repository;
using System.Net;
using Xunit;

namespace Selah.API.IntegrationTests.ControllerTests
{
    public class AppUserControllerTests : IAsyncLifetime
    {
        private SelahApiTestFactory _testFactory;
        private readonly BaseRepository _baseRepository;
        private readonly AppUserRepository _userRepository;

        private AppUser _testUser = new AppUser { };
        private HttpClient _testClient;
        private string jwt;

        public AppUserControllerTests()
        {
            _testFactory = new SelahApiTestFactory();
            _testClient = _testFactory.CreateClient();
            _baseRepository = DatabaseHelpers.CreateBaseRepository();
            _userRepository = new AppUserRepository(_baseRepository);
        }


        [Fact]
        public async Task CreateUserEndpoint_Should_Return_400_For_Invalid_Request()
        {
            //Arrange
            CreateUserCommand command = new CreateUserCommand
            {
            };

            //Act
            var response = await _testFactory.PostAsync<CreateUserCommand>(command, _testClient, "api/v1/users");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateUserEndpoint_Should_Return_200_For_Valid_Request()
        {
            //Arrange
            CreateUserCommand command = new CreateUserCommand
            {
                Email = $"{Guid.NewGuid()}@selah.com",
                FirstName = "Test",
                LastName = "User",
                UserName = $"{Guid.NewGuid()}",
                Password = "password"
            };

            //Act
            var response = await _testFactory.PostAsync<CreateUserCommand>(command, _testClient, "api/v1/users");

            // Assert
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