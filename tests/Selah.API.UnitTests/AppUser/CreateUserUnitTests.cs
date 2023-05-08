using Moq;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;
using FluentAssertions;
using Selah.Application.Commands.AppUser;
using AutoMapper.Execution;

namespace Selah.API.UnitTests.AppUser
{
    public class CreateUserUnitTests
    {
        public Mock<IAppUserRepository> _mockUserRepository = new Mock<IAppUserRepository>();

        [Fact]
        public async Task ReturnValidationErrorWhenFieldsEmpty()
        {
            //Arrange
            var command = new CreateUserCommand
            {
                CreatedUser = new Domain.Data.Models.ApplicationUser.AppUserCreate
                {
                    Email = "",
                    Password = "",
                    FirstName = "",
                    LastName = ""
                }
            };
            var validator = new CreateUserCommand.Validator(_mockUserRepository.Object);
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
                CreatedUser = new Domain.Data.Models.ApplicationUser.AppUserCreate
                {
                    Email = "test@selah.com",
                    Password = "secret",
                    FirstName = "Test",
                    LastName = "User"
                }
            };
            _mockUserRepository.Setup(x => x.GetUser(It.IsAny<string>())).ReturnsAsync(new Domain.Data.Models.ApplicationUser.AppUser { });
            var validator = new CreateUserCommand.Validator(_mockUserRepository.Object);
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
                CreatedUser = new Domain.Data.Models.ApplicationUser.AppUserCreate
                {
                    Email = "test@selah.com",
                    Password = "secret",
                    FirstName = "Test",
                    LastName = "User"
                }
            };

            _mockUserRepository.Setup(x => x.GetUser(It.IsAny<string>())).ReturnsAsync(null as Domain.Data.Models.ApplicationUser.AppUser);
            
            _mockUserRepository.Setup(x => x.CreateUser(command.CreatedUser)).ReturnsAsync(Guid.NewGuid);
            var validator = new CreateUserCommand.Validator(_mockUserRepository.Object);
            var handler = new CreateUserCommand.Handler(_mockUserRepository.Object);

            //Act
            var validationResult = await validator.ValidateAsync(command);
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            validationResult.IsValid.Should().BeTrue();
            result.Item2.Should().BeNull();
            result.Item1.Email.Should().Be("test@selah.com");
            result.Item1.FirstName.Should().Be("Test");
            result.Item1.LastName.Should().Be("User");
        }
    }
}
