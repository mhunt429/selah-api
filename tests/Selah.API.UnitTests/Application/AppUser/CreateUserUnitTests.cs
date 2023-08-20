using FluentAssertions;
using Selah.Application.Commands.AppUser;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Domain.Data.Models.Authentication;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;
using NSubstitute;

namespace Selah.API.UnitTests.AppUser;

public class CreateUserUnitTests
{
    private readonly IAppUserRepository _mockUserRepository;
    private readonly ISecurityService _mockSecurityService;
    private readonly IAuthenticationService _mockAuthService;

    public CreateUserUnitTests()
    {
        _mockUserRepository = Substitute.For<IAppUserRepository>();
        _mockSecurityService = Substitute.For<ISecurityService>();
        _mockAuthService = Substitute.For<IAuthenticationService>();

        _mockAuthService.GenerateJwt(Arg.Any<UserViewModel>()).Returns(new JwtResponse
        {
            AccessToken = "token",
            ExpirationTs = DateTime.Now.AddSeconds(86400)
        });

        _mockSecurityService.EncodeHashId(Arg.Any<int>()).Returns("abc123");
    }

    [Fact]
    public async Task ReturnValidationErrorWhenFieldsEmpty()
    {
        //Arrange
        var command = new CreateUserCommand
        {
            Email = "",
            Password = "",
            FirstName = "",
            LastName = ""
        };
        var validator = new CreateUserCommand.Validator(_mockUserRepository);
        //Act
        var result = await validator.ValidateAsync(command);

        //Assert
        result.Errors.Select(x => x.ErrorMessage.Should().Contain("Email must not be empty."));
        result.Errors.Select(x => x.ErrorMessage.Should().Contain("Password must not be empty."));
        result.Errors.Select(x => x.ErrorMessage.Should().Contain("FirstName must not be empty."));
        result.Errors.Select(x => x.ErrorMessage.Should().Contain("LastName must not be empty."));
    }

    [Fact]
    public async Task ReturnValidationErrorWhenEmailIsNotUnique()
    {
        //Arrange
        var command = new CreateUserCommand
        {
            Email = "test@selah.com",
            Password = "secret",
            FirstName = "Test",
            LastName = "User"
        };
        _mockUserRepository.GetUser(Arg.Any<string>())
            .Returns(new Domain.Data.Models.ApplicationUser.AppUser());

        var validator = new CreateUserCommand.Validator(_mockUserRepository);
        //Act
        var result = await validator.ValidateAsync(command);

        //Assert
        result.Errors.Select(x => x.ErrorMessage.Should().Contain("An account with this email already exists."));
    }

    [Fact]
    public async Task CanCreateUserWhenModelIsValid()
    {
        //Arrange
        var command = new CreateUserCommand
        {
            Email = "test@selah.com",
            Password = "secret",
            FirstName = "Test",
            LastName = "User"
        };

        var user = new AppUserCreate
        {
            Email = "test@selah.com",
            Password = "secret",
            FirstName = "Test",
            LastName = "User"
        };
        _mockUserRepository.GetUser(Arg.Any<string>()).Returns(null as Domain.Data.Models.ApplicationUser.AppUser);
        _mockUserRepository.CreateUser(user).Returns(1);

        var validator = new CreateUserCommand.Validator(_mockUserRepository);
        var handler = new CreateUserCommand.Handler(_mockUserRepository, _mockSecurityService,
            _mockAuthService);

        //Act
        var validationResult = await validator.ValidateAsync(command);
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        validationResult.IsValid.Should().BeTrue();
        result.Item2.Should().BeNull();
        result.Item1.User.Should().NotBeNull();
        result.Item1.User.Email.Should().Be("test@selah.com");
        result.Item1.User.FirstName.Should().Be("Test");
        result.Item1.User.LastName.Should().Be("User");

        result.Item1.AccessToken.Should().Be("token");
        result.Item1.ExpirationTs.Should().BeAfter(DateTime.MinValue);
    }
}