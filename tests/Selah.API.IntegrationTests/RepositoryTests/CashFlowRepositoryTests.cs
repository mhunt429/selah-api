using Selah.Infrastructure.Repository;
using Selah.Infrastructure;
using Xunit;
using Selah.API.IntegrationTests.helpers;
using Selah.Infrastructure.Repository.Interfaces;
using FluentAssertions;
using Selah.Domain.Data.Models.CashFlow;
using Microsoft.Extensions.Logging;
using Moq;

namespace Selah.API.IntegrationTests.RepositoryTests
{
    public class CashFlowRepositoryTests : IAsyncLifetime
    {
        private readonly BaseRepository _baseRepository;
        private readonly AppUserRepository _userRepository;
        private readonly ICashFlowRepository _cashFlowRepository;
        private readonly Mock<ILogger<BaseRepository>> _loggerMock;
        private Guid _userId = Guid.Empty;
        public CashFlowRepositoryTests()
        {
            var dbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                dbConnectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=65432;Database=postgres";
            }
            _loggerMock = new Mock<ILogger<BaseRepository>>();

            _baseRepository = new BaseRepository(new NpgsqlConnectionFactory(dbConnectionString), _loggerMock.Object);
            _userRepository = new AppUserRepository(_baseRepository);
            _cashFlowRepository = new CashFlowRepository(_baseRepository);
        }

        [Fact]
        public async Task CreateIncomeStatement_Should_Return_The_New_Id()
        {
            var dataToSave = new IncomeStatementCreate
            {
                UserId = _userId,
                StatementStartDate = DateTime.Now,
                StatementEndDate = DateTime.Now.AddDays(14),
                TotalPay = 500
            };
            var result = await _cashFlowRepository.CreateIncomeStatement(dataToSave);
            result.Should().BeGreaterThan(0);
        }

        public async Task InitializeAsync()
        {
            _userId = await DatabaseHelpers.CreateUser(_userRepository);
        }

        public async Task DisposeAsync()
        {
            await DatabaseHelpers.RunSingleDelete(_baseRepository, @"DELETE FROM income_statement 
                WHERE user_id = @user_id",
                new { user_id = _userId }
            );
            await DatabaseHelpers.DeleteUser(_baseRepository, _userId);
        }
    }
}
