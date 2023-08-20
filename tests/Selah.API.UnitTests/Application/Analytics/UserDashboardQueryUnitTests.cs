using FluentAssertions;
using Selah.Application.Queries.Analytics;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.Analytics.Dashboard;
using Selah.Domain.Data.Models.Transactions.Sql;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;
using NSubstitute;

namespace Selah.Application.UnitTests.Analytics;

public class UserDashboardQueryUnitTests
{
    private readonly ITransactionRepository _transactionRepositoryMock;
    private readonly ISecurityService _securityServiceMock;

    public UserDashboardQueryUnitTests()
    {
        _transactionRepositoryMock = Substitute.For<ITransactionRepository>();
        _securityServiceMock = Substitute.For<ISecurityService>();

        _transactionRepositoryMock.GetRecentTransactions(1).Returns(new List<RecentTransactionSql>());

        _transactionRepositoryMock.GetTransactionSummaryByDateRange(1, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(new List<TransactionSummarySql>());

        _transactionRepositoryMock.GetEmptyTransactionSummary(Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(
            new List<TransactionSummarySql>
            {
                new()
                {
                    TransactionDate = DateTime.UtcNow,
                    TotalAmount = 0,
                    Count = 0
                }
            });
    }

    [Fact]
    public async Task Handle_ShouldReturnDashboardSummary_WhenValidQuery()
    {
        // Arrange
        var query = new UserDashboardQuery { UserId = "ABC123" };
        var handler = new UserDashboardQuery.Handler(_transactionRepositoryMock, _securityServiceMock);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.RecentTransactions);
        Assert.NotEmpty(result.LastMonthSpending);
        Assert.NotEmpty(result.CurrentMonthSpending);
        Assert.NotNull(result.UpcomingTransactions);
        Assert.NotNull(result.PortfolioSummary);
        Assert.NotNull(result.NetWorthSummary);

        foreach (var item in result.LastMonthSpending)
        {
            item.Count.Should().Be(0);
            item.TotalAmount.Should().Be(0);
            item.TransactionDate.Should().BeAfter(DateTime.MinValue);
        }

        foreach (var item in result.CurrentMonthSpending)
        {
            item.Count.Should().Be(0);
            item.TotalAmount.Should().Be(0);
            item.TransactionDate.Should().BeAfter(DateTime.MinValue);
        }
    }
}