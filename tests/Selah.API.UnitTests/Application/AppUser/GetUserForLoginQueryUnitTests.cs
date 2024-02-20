using FluentAssertions;
using FluentValidation.TestHelper;
using Selah.Application.Queries.ApplicationUser;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Domain.Data.Models.Authentication;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;
using NSubstitute;

namespace Selah.Application.UnitTests.AppUser;

public class GetUserForLoginQueryUnitTests
{
    private IAppUserRepository _appUserRepoMock;
    private readonly IAuthenticationService _authServiceMock;
    private readonly ISecurityService _securityServiceMock;

    public GetUserForLoginQueryUnitTests()
    {
        _appUserRepoMock = Substitute.For<IAppUserRepository>();
        _authServiceMock = Substitute.For<IAuthenticationService>();
        _securityServiceMock = Substitute.For<ISecurityService>();
        _authServiceMock.GenerateJwt(Arg.Any<UserViewModel>()).Returns(new JwtResponse
        {
            AccessToken = "token",
            ExpirationTs = DateTime.Now.AddSeconds(86400)
        });

        _securityServiceMock.EncodeHashId(Arg.Any<long>()).Returns("abc123");
    }

    [Theory]
    [InlineData("", null)]
    [InlineData(null, "")]
    public void GetUserForLoginQuery_ValidatesAgainstInvalidModel(string emailOrUsername, string password)
    {
        //Arrange
        var query = new GetUserForLoginQuery { EmailOrUsername = emailOrUsername, Password = password };
        var validator = new GetUserForLoginQuery.Validator();

        //Act
        var result = validator.TestValidate(query);
        //Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.EmailOrUsername);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void GetUserForLoginQuery_ShouldValidateModel()
    {
        //Arrange
        var query = new GetUserForLoginQuery { EmailOrUsername = "testing", Password = "testing" };
        var validator = new GetUserForLoginQuery.Validator();
        //Act
        var result = validator.TestValidate(query);
        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Count.Should().Be(0);
    }

    [Fact]
    public async Task GetUserForLoginQueryUnitTests_Handler_Returns_User_With_Token()
    {
        //Arrange
        _appUserRepoMock.GetUser(Arg.Any<string>()).Returns(
            new Domain.Data.Models.ApplicationUser.AppUser
            {
                Id = 1,
                Email = "test@test.com",
                Password =
                    "$2a$11$Lr3xlczLVYzspbYZNZiZIuMdW0f3xNMcoJZuPfxV9Z3YiNUb4jW8e", //Bcrypt for super-secret123 DO NOT ACTUALLY USE THIS FOR A REAL PASSWORD
                UserName = "test",
                FirstName = "Test",
                LastName = "User",
                DateCreated = DateTime.Now
            });
        var query = new GetUserForLoginQuery { EmailOrUsername = "testing", Password = "super-secret123" };
        var handler = new GetUserForLoginQuery.Handler(_appUserRepoMock, _authServiceMock,
            _securityServiceMock);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.Should().NotBeNull();
        result.User.Should().NotBeNull();
        result.User.Id.Should().Be("abc123");
        result.User.Email.Should().Be("test@test.com");
        result.User.UserName.Should().Be("test");
        result.User.FirstName.Should().Be("Test");
        result.User.LastName.Should().Be("User");
        result.User.DateCreated.Should().BeAfter(DateTime.MinValue);

        result.AccessToken.Should().Be("token");
        result.ExpirationTs.Should().BeAfter(DateTime.MinValue);
    }

    [Fact]
    public async Task GetUserForLoginQueryUnitTests_Handler_NullWhenIdIsNotFound()
    {
        //Arrange
        _appUserRepoMock.GetUser(Arg.Any<string>()).Returns(
            null as Domain.Data.Models.ApplicationUser.AppUser);
        
        var query = new GetUserForLoginQuery { EmailOrUsername = "testing", Password = "super-secret123" };
        
        var handler = new GetUserForLoginQuery.Handler(_appUserRepoMock, _authServiceMock,
            _securityServiceMock);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetUserForLoginQueryUnitTests_Handler_NullWhenPasswordIsInvalid()
    {
        //Arrange
        _appUserRepoMock.GetUser(Arg.Any<string>()).Returns(
            new Domain.Data.Models.ApplicationUser.AppUser
            {
                Id = 1,
                Email = "test@test.com",
                Password =
                    "$2a$11$Lr3xlczLVYzspbYZNZiZIuMdW0f3xNMcoJZuPfxV9Z3YiNUb4jW8e", //Bcrypt for super-secret123 DO NOT ACTUALLY USE THIS FOR A REAL PASSWORD
                UserName = "test",
                FirstName = "Test",
                LastName = "User",
                DateCreated = DateTime.Now
            });
        var query = new GetUserForLoginQuery { EmailOrUsername = "testing", Password = "bad password" };
        var handler = new GetUserForLoginQuery.Handler(_appUserRepoMock, _authServiceMock,
            _securityServiceMock);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.Should().BeNull();
    }
}