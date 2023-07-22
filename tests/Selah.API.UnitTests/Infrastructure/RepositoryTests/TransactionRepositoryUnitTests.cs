using Dapper;
using FluentAssertions;
using Moq;
using Selah.Domain.Data.Models.Transactions;
using Selah.Domain.Data.Models.Transactions.Sql;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;

namespace Selah.Application.UnitTests.Infrastructure.RepositoryTests;

public class TransactionRepositoryUnitTests
{
    private readonly Mock<IBaseRepository> _baseRepository;
    private readonly ITransactionRepository _transactionRepository;

    public TransactionRepositoryUnitTests()
    {
        _baseRepository = new Mock<IBaseRepository>();
        _baseRepository.Setup(x =>
                x.AddAsync<int>(It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync(1);
        _baseRepository.Setup(x =>
                x.UpdateAsync(It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync(true);

        _baseRepository.Setup(x =>
                x.DeleteAsync(It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync(true);
        _baseRepository.Setup(x => x.AddManyAsync<TransactionLineItemCreate>(It.IsAny<string>(),
            It.IsAny<List<DynamicParameters>>())).ReturnsAsync(2);


        _transactionRepository = new TransactionRepository(_baseRepository.Object);
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
        _baseRepository.Setup(x => x.GetAllAsync<UserTransactionCategory>(It.IsAny<string>(),
            It.IsAny<object>())).ReturnsAsync(new List<UserTransactionCategory>());

        var result = await _transactionRepository.GetTransactionCategoriesByUser(1);
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTransactionCategoryById_ShouldReturnEmptyList()
    {
        _baseRepository.Setup(x => x.GetAllAsync<UserTransactionCategory>(It.IsAny<string>(),
            It.IsAny<object>())).ReturnsAsync(new List<UserTransactionCategory>());

        var result = await _transactionRepository.GetTransactionCategoryById(1, 1);
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTransactionCategoriesByUserAndName_ShouldReturnEmptyList()
    {
        _baseRepository.Setup(x => x.GetAllAsync<UserTransactionCategory>(It.IsAny<string>(),
            It.IsAny<object>())).ReturnsAsync(new List<UserTransactionCategory>());

        var result = await _transactionRepository.GetTransactionCategoriesByUserAndName(1, "test category");
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task InsertTransaction_ShoudlReturnNewId()
    {
        var result = await _transactionRepository.InsertTransaction(new TransactionCreate());
        result.Should().Be(1);
    }

    [Fact]
    public async Task GetItemizedTransactionAsync_ShouldReturnEmptyList()
    {
        _baseRepository.Setup(x => x.GetAllAsync<ItemizedTransactionSql>(It.IsAny<string>(),
            It.IsAny<object>())).ReturnsAsync(new List<ItemizedTransactionSql>());
        
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
        _baseRepository.Setup(x => x.GetAllAsync<RecentTransactionSql>(It.IsAny<string>(),
            It.IsAny<object>())).ReturnsAsync(new List<RecentTransactionSql>());
        
        var result = await _transactionRepository.GetRecentTransactions(1);
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTransactionSummaryByDateRange_ShouldReturnEmptyList()
    {
        _baseRepository.Setup(x => x.GetAllAsync<RecentTransactionSql>(It.IsAny<string>(),
            It.IsAny<object>())).ReturnsAsync(new List<RecentTransactionSql>());
        
        var result = await _transactionRepository.GetRecentTransactions(1);
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}