using FluentAssertions;
using NSubstitute;
using Selah.Domain.Data.Models.Banking;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;

namespace Selah.Application.UnitTests.Infrastructure.RepositoryTests;

public class BankingRepositoryUnitTests
{
    private readonly IBaseRepository _baseRepository;
    private IBankingRepository _bankingRepository;

    public BankingRepositoryUnitTests()
    {
        _baseRepository = Substitute.For<IBaseRepository>();

        _baseRepository.GetFirstOrDefaultAsync<BankAccountSql>(Arg.Any<string>(), Arg.Any<object>())
            .Returns(new BankAccountSql());

        _baseRepository.GetAllAsync<BankAccountSql>(Arg.Any<string>(), Arg.Any<object>())
            .Returns(new List<BankAccountSql> {  new BankAccountSql { Id = 1 }  });

        _bankingRepository = new BankingRepository(_baseRepository);
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