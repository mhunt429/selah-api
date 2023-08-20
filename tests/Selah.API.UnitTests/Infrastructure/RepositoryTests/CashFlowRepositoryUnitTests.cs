using Dapper;
using FluentAssertions;
using NSubstitute;
using Selah.Domain.Data.Models.CashFlow;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;

namespace Selah.Application.UnitTests.Infrastructure.RepositoryTests;

public class CashFlowRepositoryUnitTests
{
    private readonly IBaseRepository _baseRepository;
    private readonly ICashFlowRepository _cashFlowRepository;

    public CashFlowRepositoryUnitTests()
    {
        _baseRepository = Substitute.For<IBaseRepository>();
        _baseRepository.AddAsync<int>(Arg.Any<string>(), Arg.Any<Object>())
            .Returns(1);
        _baseRepository.GetAllAsync<IncomeStatement>(Arg.Any<string>(),
            Arg.Any<Object>()).Returns(new List<IncomeStatement>());
        _baseRepository.AddManyAsync<IncomeStatementDeduction>(Arg.Any<string>(),
            Arg.Any<List<DynamicParameters>>()).Returns(2);

        _cashFlowRepository = new CashFlowRepository(_baseRepository);
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