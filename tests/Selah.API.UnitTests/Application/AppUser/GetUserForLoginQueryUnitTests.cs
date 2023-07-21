using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using Selah.Application.Queries.ApplicationUser;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Domain.Data.Models.Authentication;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;

namespace Selah.Application.UnitTests.AppUser;

public class GetUserForLoginQueryUnitTests
{
    private Mock<IAppUserRepository> _appUserRepoMock = new Mock<IAppUserRepository>();
    private readonly Mock<IAuthenticationService> _authServiceMock = new Mock<IAuthenticationService>();
    private readonly Mock<ISecurityService> _securityServiceMock = new Mock<ISecurityService>();

    public GetUserForLoginQueryUnitTests()
    {
        _authServiceMock.Setup(x => x.GenerateJwt(It.IsAny<UserViewModel>())).Returns(new JwtResponse
        {
            AccessToken = "token",
            ExpirationTs = DateTime.Now.AddSeconds(86400)
        });

        _securityServiceMock.Setup(x => x.EncodeHashId(It.IsAny<int>())).Returns("abc123");
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
        _appUserRepoMock.Setup(x => x.GetUser(It.IsAny<string>())).ReturnsAsync(
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
        var handler = new GetUserForLoginQuery.Handler(_appUserRepoMock.Object, _authServiceMock.Object,
            _securityServiceMock.Object);

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
        _appUserRepoMock.Setup(x => x.GetUser(It.IsAny<string>())).ReturnsAsync(
        null as Domain.Data.Models.ApplicationUser.AppUser);
        var query = new GetUserForLoginQuery { EmailOrUsername = "testing", Password = "super-secret123" };
        var handler = new GetUserForLoginQuery.Handler(_appUserRepoMock.Object, _authServiceMock.Object,
            _securityServiceMock.Object);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task GetUserForLoginQueryUnitTests_Handler_NullWhenPasswordIsInvalid()
    {
        //Arrange
        _appUserRepoMock.Setup(x => x.GetUser(It.IsAny<string>())).ReturnsAsync(
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
        var handler = new GetUserForLoginQuery.Handler(_appUserRepoMock.Object, _authServiceMock.Object,
            _securityServiceMock.Object);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.Should().BeNull();
    }
    
}