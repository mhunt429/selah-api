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
        private long _incomeStatementId;
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
            await CreateTestUser();
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

        [Fact]
        public async Task InsertIncomeStatementDeductions_Should_Return_Number_Of_Inserted_Rows()
        {
            await SetupData();
            var deductions = new List<IncomeStatementDeduction>
            {
                new IncomeStatementDeduction
                {
                    StatementId = _incomeStatementId,
                    DeductionName = "Federal Taxes",
                    Amount = 500
                },
                new IncomeStatementDeduction
                {
                    StatementId = _incomeStatementId,
                    DeductionName = "Medical Insurance",
                    Amount = 500
                }
            };

            var result = await _cashFlowRepository.InsertIncomeStatementDeductions(deductions);
            result.Should().Be(2);
        }

        public async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await DatabaseHelpers.RunSingleDelete(_baseRepository, @"DELETE FROM 
                income_statement_deduction
                WHERE statement_id = @statement_id",
                new { statement_id = _incomeStatementId });

            await DatabaseHelpers.RunSingleDelete(_baseRepository, @"DELETE FROM income_statement 
                WHERE user_id = @user_id",
                new { user_id = _userId }
            );
            await DatabaseHelpers.DeleteTestUsers(_baseRepository);
        }

        private async Task SetupData()
        {
            _userId = await DatabaseHelpers.CreateUser(_userRepository);
            var dataToSave = new IncomeStatementCreate
            {
                UserId = _userId,
                StatementStartDate = DateTime.Now,
                StatementEndDate = DateTime.Now.AddDays(14),
                TotalPay = 500
            };
            _incomeStatementId = await _cashFlowRepository.CreateIncomeStatement(dataToSave);
        }

        private async Task CreateTestUser()
        {
            _userId = await DatabaseHelpers.CreateUser(_userRepository);
        }
    }
}
