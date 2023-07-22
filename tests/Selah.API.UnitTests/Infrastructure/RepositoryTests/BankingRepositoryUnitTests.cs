using FluentAssertions;
using Moq;
using Selah.Domain.Data.Models.Banking;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;

namespace Selah.Application.UnitTests.Infrastructure.RepositoryTests;

public class BankingRepositoryUnitTests
{
    private readonly Mock<IBaseRepository> _baseRepository = new Mock<IBaseRepository>();
    private IBankingRepository _bankingRepository;

    public BankingRepositoryUnitTests()
    {
        _baseRepository.Setup(x =>
                x.GetFirstOrDefaultAsync<BankAccount>(It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync(new BankAccount());

        _baseRepository.Setup(x =>
                x.GetAllAsync<BankAccount>(It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync(new List<BankAccount> { { new BankAccount { Id = 1 } } });

        _bankingRepository = new BankingRepository(_baseRepository.Object);
    }

    [Fact]
    public async Task GetAccounts_ShouldReturnCollectionOfAccounts()
    {
        var result = await _bankingRepository.GetAccounts(1, 25, 1);
        result.Should().NotBeEmpty();
        result.Count().Should().Be(1);
        result.FirstOrDefault().Id.Should().Be(1);
    }

    [Fact]
    public async Task GetAccountsByInstitution_ShouldReturnCollectionOfAccounts()
    {
        var result = await _bankingRepository.GetAccountsByInstitutionId(1);
        result.Should().NotBeEmpty();
        result.Count().Should().Be(1);
        result.FirstOrDefault().Id.Should().Be(1);
    }

    [Fact]
    public async Task GetAccountById_ShouldReturnOneAccount()
    {
        var result = await _bankingRepository.GetAccountById(1);
        result.Should().NotBeNull();
    }
}