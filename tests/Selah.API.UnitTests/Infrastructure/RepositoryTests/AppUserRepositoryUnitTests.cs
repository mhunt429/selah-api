using FluentAssertions;
using NSubstitute;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;

namespace Selah.Application.UnitTests.Infrastructure.RepositoryTests;

using AppUser = Selah.Domain.Data.Models.ApplicationUser.AppUser;

public class AppUserRepositoryUnitTests
{
    private readonly IBaseRepository _baseRepository;
    private IAppUserRepository _userRepository;

    public AppUserRepositoryUnitTests()
    {
        //Initializes base repository
        _baseRepository = Substitute.For<IBaseRepository>();

        _baseRepository.GetFirstOrDefaultAsync<AppUser>(Arg.Any<string>(), Arg.Any<object>())
            .Returns(new AppUser());

        _baseRepository.AddAsync<int>(Arg.Any<string>(), Arg.Any<object>())
            .Returns(1);

        _baseRepository.UpdateAsync(Arg.Any<string>(), Arg.Any<object>())
            .Returns(true);

        _baseRepository.DeleteAsync(Arg.Any<string>(), Arg.Any<object>())
            .Returns(true);

        _userRepository = new AppUserRepository(_baseRepository);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnUser()
    {
        //Act
        var result = await _userRepository.GetUser(1);

        //Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Get_ShouldReturnUser()
    {
        //Act
        var result = await _userRepository.GetUser("test@test.com");

        //Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateUser_ShouldReturnNewId()
    {
        //act
        var result = await _userRepository.CreateUser(new AppUserCreate());
        //assert
        result.Should().Be(1);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnTrue()
    {
        var result = await _userRepository.UpdateUser(new AppUserUpdate(), 1);
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task DeleteUser_ShouldReturnTrue()
    {
        var result = await _userRepository.DeleteUser(1);
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task UpdatePassword_ShouldReturnTrue()
    {
        var result = await _userRepository.UpdatePassword(1, "my password");
        result.Should().BeTrue();
    }
}