using FluentValidation;
using Moq;
using Selah.Application.Queries.Analytics;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.Analytics.Dashboard;
using Selah.Domain.Data.Models.Transactions.Sql;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;

namespace Selah.Application.UnitTests.Analytics;

public class UserDashboardQueryUnitTests
{
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<ISecurityService> _securityServiceMock;
    public UserDashboardQueryUnitTests()
    {
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _securityServiceMock = new Mock<ISecurityService>();
    }

    [Fact]
    public async Task Handle_ShouldReturnDashboardSummary_WhenValidQuery()
    {
        // Arrange
        var query = new UserDashboardQuery { UserId = "ABC123" };
        var handler = new UserDashboardQuery.Handler(_transactionRepositoryMock.Object, _securityServiceMock.Object);

        _transactionRepositoryMock.Setup(x => x.GetRecentTransactions(1))
            .ReturnsAsync(new List<RecentTransactionSql>());

        _transactionRepositoryMock.Setup(x =>
                x.GetTransactionSummaryByDateRange(1, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<TransactionSummarySql>());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.RecentTransactions);
        Assert.NotNull(result.LastMonthSpending);
        Assert.NotNull(result.CurrentMonthSpending);
        Assert.NotNull(result.UpcomingTransactions);
        Assert.NotNull(result.PortfolioSummary);
        Assert.NotNull(result.NetWorthSummary);
    }
}