using Dapper;
using FluentAssertions;
using NSubstitute;
using Selah.Domain.Data.Models.Transactions;
using Selah.Domain.Data.Models.Transactions.Sql;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;

namespace Selah.Application.UnitTests.Infrastructure.RepositoryTests;

public class TransactionRepositoryUnitTests
{
    private readonly IBaseRepository _baseRepository;
    private readonly ITransactionRepository _transactionRepository;

    public TransactionRepositoryUnitTests()
    {
        _baseRepository = Substitute.For<IBaseRepository>();
        _baseRepository.AddAsync<long>(Arg.Any<string>(), Arg.Any<object>())
            .Returns(1);
        _baseRepository.UpdateAsync(Arg.Any<string>(), Arg.Any<object>())
            .Returns(true);
        _baseRepository.DeleteAsync(Arg.Any<string>(), Arg.Any<object>())
            .Returns(true);
        _baseRepository.AddManyAsync<TransactionLineItemCreate>(Arg.Any<string>(),
            Arg.Any<List<DynamicParameters>>()).Returns(2);


        _transactionRepository = new TransactionRepository(_baseRepository);
    }

    [Fact]
    public async Task CreateTransactionCategory_ShouldReturnNewId()
    {
        var result = await _transactionRepository.CreateTransactionCategory(new UserTransactionCategoryCreate());
        result.Should().Be(1);
    }

    [Fact]
    public async Task GetTransactionCategoriesByUser_ShouldReturnEmptyList()
    {
        _baseRepository.GetAllAsync<UserTransactionCategory>(Arg.Any<string>(),
            Arg.Any<object>()).Returns(new List<UserTransactionCategory>());

        var result = await _transactionRepository.GetTransactionCategoriesByUser(1);
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTransactionCategoryById_ShouldReturnEmptyList()
    {
        _baseRepository.GetAllAsync<UserTransactionCategory>(Arg.Any<string>(),
            Arg.Any<object>()).Returns(new List<UserTransactionCategory>());

        var result = await _transactionRepository.GetTransactionCategoryById(1, 1);
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTransactionCategoriesByUserAndName_ShouldReturnEmptyList()
    {
        _baseRepository.GetAllAsync<UserTransactionCategory>(Arg.Any<string>(),
            Arg.Any<object>()).Returns(new List<UserTransactionCategory>());

        var result = await _transactionRepository.GetTransactionCategoriesByUserAndName(1, "test category");
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task InsertTransaction_ShoudlReturnNewId()
    {
        var result = await _transactionRepository.InsertTransaction(new TransactionCreateSql());
        result.Should().Be(1);
    }

    [Fact]
    public async Task GetItemizedTransactionAsync_ShouldReturnEmptyList()
    {
        _baseRepository.GetAllAsync<ItemizedTransactionSql>(Arg.Any<string>(),
            Arg.Any<object>()).Returns(new List<ItemizedTransactionSql>());

        var result = await _transactionRepository.GetItemizedTransactionAsync(1);
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task InsertTransactionLineItems_ShouldReturnNumberOfRows()
    {
        var result = await _transactionRepository.InsertTransactionLineItems(new List<TransactionLineItemCreate>());
        result.Should().Be(2);
    }

    [Fact]
    public async Task GetRecentTransactions_ShouldReturnEmptyList()
    {
        _baseRepository.GetAllAsync<RecentTransactionSql>(Arg.Any<string>(),
            Arg.Any<object>()).Returns(new List<RecentTransactionSql>());

        var result = await _transactionRepository.GetRecentTransactions(1);
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTransactionSummaryByDateRange_ShouldReturnEmptyList()
    {
        _baseRepository.GetAllAsync<RecentTransactionSql>(Arg.Any<string>(),
            Arg.Any<object>()).Returns(new List<RecentTransactionSql>());

        var result = await _transactionRepository.GetRecentTransactions(1);
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTransactionTotalsByCategory_ShouldReturnNonEmptyList()
    {
        _baseRepository.GetAllAsync<TransactionAmountByCategorySql>(Arg.Any<string>(),
            Arg.Any<object>()).Returns(new List<TransactionAmountByCategorySql>()
        {
            new()
            {
                Id = 1,
                Name = "Category 1",
                Total = 100
            },
            new()
            {
                Id = 2,
                Name = "Category 2",
                Total = 100
            }
        });

        var result = await _transactionRepository.GetTransactionTotalsByCategory(1);
        result.Should().NotBeEmpty();

        foreach (var item in result)
        {
            item.Id.Should().NotBe(0);
            item.Name.Should().NotBeEmpty();
            item.Total.Should().Be(100);
        }
    }
}