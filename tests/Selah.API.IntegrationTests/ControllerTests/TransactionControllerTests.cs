using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using Selah.API.IntegrationTests.helpers;
using Selah.Application.Commands.AppUser;
using Selah.Application.Queries;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Domain.Data.Models.Authentication;
using Selah.Infrastructure.Repository;
using Xunit;

namespace Selah.API.IntegrationTests.ControllerTests;

public class TransactionControllerTests : IAsyncLifetime
{
    private SelahApiTestFactory _testFactory;
    private readonly BaseRepository _baseRepository;
    private readonly AppUserRepository _userRepository;
    private AppUser _testUser = new AppUser { };
    private HttpClient _testClient;
    private string jwt;

    public TransactionControllerTests()
    {
        _testFactory = new SelahApiTestFactory();
        _testClient = _testFactory.CreateClient();
        _baseRepository = DatabaseHelpers.CreateBaseRepository();
        _userRepository = new AppUserRepository(_baseRepository);
    }
    [Fact]
    public async Task TransactionTotalsByCategoryQuery_Unauthorized_RequestReturns401()
    {
        var response = await _testFactory.GetAsync<TransactionTotalsByCategoryQuery>(_testClient, $"api/v1/users/{_testUser.Id}/transactions/categories/totals");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task TransactionTotalsByCategoryQuery_InvalidUserId_RequestReturns403()
    {
        _testClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        var response = await _testFactory.GetAsync<TransactionTotalsByCategoryQuery>(_testClient, $"api/v1/users/{0}/transactions/categories/totals");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task TransactionTotalsByCategoryQuery_ValidTokenAndUserId_RequestReturns200()
    {
        _testClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        var hashId = _testFactory.GetEncodedToken(_testUser.Id);
        var response = await _testFactory.GetAsync<CreateUserCommand>(_testClient, $"api/v1/users/{hashId}/transactions/categories/totals");
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