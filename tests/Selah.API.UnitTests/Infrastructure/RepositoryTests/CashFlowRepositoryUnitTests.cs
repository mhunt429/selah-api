using Dapper;
using FluentAssertions;
using Moq;
using Selah.Domain.Data.Models.CashFlow;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;

namespace Selah.Application.UnitTests.Infrastructure.RepositoryTests;

public class CashFlowRepositoryUnitTests
{
    private readonly Mock<IBaseRepository> _baseRepository;
    private readonly ICashFlowRepository _cashFlowRepository;

    public CashFlowRepositoryUnitTests()
    {
        _baseRepository = new Mock<IBaseRepository>();
        _baseRepository.Setup(x => x.AddAsync<int>(It.IsAny<string>(), It.IsAny<Object>()))
            .ReturnsAsync(1);
        _baseRepository.Setup(x => x.GetAllAsync<IncomeStatement>(It.IsAny<string>(),
            It.IsAny<Object>())).ReturnsAsync(new List<IncomeStatement>());
        _baseRepository.Setup(x => x.AddManyAsync<IncomeStatementDeduction>(It.IsAny<string>(),
            It.IsAny<List<DynamicParameters>>())).ReturnsAsync(2);

        _cashFlowRepository = new CashFlowRepository(_baseRepository.Object);
    }

    [Fact]
    public async Task CreateIncomeStatement_ShouldReturnNewId()
    {
        var result = await _cashFlowRepository.CreateIncomeStatement(new IncomeStatementCreate());
        result.Should().Be(1);
    }

    [Fact]
    public async Task GetIncomeStatementsByUser_ShouldReturnListOfStatements()
    {
        var result = await _cashFlowRepository.GetIncomeStatementsByUser(1, 1, 1);
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task InsertIncomeStatementDeductions_ShouldReturnNumberOfInsertedRows()
    {
        var result = await _cashFlowRepository.InsertIncomeStatementDeductions(new List<IncomeStatementDeduction>());
        result.Should().Be(2);
    }

    [Fact]
    public async Task GetDeductionsByStatement_ShouldReturnListOfStatements()
    {
        var result = await _cashFlowRepository.GetDeductionsByStatement(1, 1);
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}