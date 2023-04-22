using FluentValidation;
using Moq;
using Selah.Application.Queries.Analytics;
using Selah.Domain.Data.Models.Analytics.Dashboard;
using Selah.Domain.Data.Models.Transactions.Sql;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;

namespace Selah.Application.UnitTests.Analytics
{
    public class UserDashboardQueryUnitTests
    {
        private Mock<ITransactionRepository> _transactionRepositoryMock;

        public UserDashboardQueryUnitTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
        }

        [Fact]
        public async Task Handle_ShouldReturnDashboardSummary_WhenValidQuery()
        {
            // Arrange
            var query = new UserDashboardQuery { UserId = Guid.NewGuid() };
            var handler = new UserDashboardQuery.Handler(_transactionRepositoryMock.Object);

            _transactionRepositoryMock.Setup(x => x.GetRecentTransactions(query.UserId))
                .ReturnsAsync(new List<RecentTransactionSql>());

            _transactionRepositoryMock.Setup(x => x.GetTransactionSummaryByDateRange(query.UserId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync((new List<TransactionSummarySql>()));

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

        [Fact]
        public void Validator_ShouldThrowValidationException_WhenUserIdEmpty()
        {
            // Arrange
            var query = new UserDashboardQuery { UserId = Guid.Empty };
            var validator = new UserDashboardQuery.Validator();

            // Act and Assert
            Assert.Throws<ValidationException>(() => validator.ValidateAndThrow(query));
        }
    }
}
