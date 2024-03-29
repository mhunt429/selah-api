﻿using Selah.Infrastructure.Repository;
using Xunit;
using Selah.API.IntegrationTests.helpers;
using Selah.Infrastructure.Repository.Interfaces;
using FluentAssertions;
using Selah.Domain.Data.Models.CashFlow;


namespace Selah.API.IntegrationTests.RepositoryTests
{
    public class CashFlowRepositoryTests : IAsyncLifetime
    {
        private readonly BaseRepository _baseRepository;
        private readonly AppUserRepository _userRepository;
        private readonly ICashFlowRepository _cashFlowRepository;

        private int _incomeStatementId;
        private int _userId = 0;

        public CashFlowRepositoryTests()
        {
            _baseRepository = DatabaseHelpers.CreateBaseRepository();
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

        [Fact]
        public async Task GetDeductionsByStatement_Should_ReturnAllDeductionsForStatement()
        {
            //Arrange
            await SetupStatementWithDeductions();

            //Act
            var result = await _cashFlowRepository.GetDeductionsByStatement(_incomeStatementId, _userId);

            //Assert
            result.Count().Should().Be(2);

            result.ElementAt(0).Id.Should().BeGreaterThan(0);
            result.ElementAt(0).StatementId.Should().Be(_incomeStatementId);
            result.ElementAt(0).DeductionName.Should().Be("Federal Taxes");
            result.ElementAt(0).Amount.Should().Be(500);

            result.ElementAt(1).Id.Should().BeGreaterThan(0);
            result.ElementAt(1).DeductionName.Should().Be("Medical Insurance");
            result.ElementAt(1).Amount.Should().Be(500);
        }

        public async Task InitializeAsync()
        {
            _userId = (await DatabaseHelpers.CreateUser(_userRepository)).Id;
            var dataToSave = new IncomeStatementCreate
            {
                UserId = _userId,
                StatementStartDate = DateTime.Now,
                StatementEndDate = DateTime.Now.AddDays(14),
                TotalPay = 500
            };
            _incomeStatementId = await _cashFlowRepository.CreateIncomeStatement(dataToSave);
        }

        public async Task DisposeAsync()
        {
            await DatabaseHelpers.RunSingleDelete(_baseRepository, @"DELETE FROM income_statement_deduction",
                new { });
            await DatabaseHelpers.RunSingleDelete(_baseRepository, @"DELETE FROM income_statement", new { });
            await DatabaseHelpers.DeleteTestUsers(_baseRepository, _userId);
        }

        private async Task CreateTestUser()
        {
            _userId = (await DatabaseHelpers.CreateUser(_userRepository)).Id;
        }

        /// <summary>
        /// Inserts test data for income statement and statement deductions
        /// </summary>
        /// <returns></returns>
        private async Task SetupStatementWithDeductions()
        {
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

            await _cashFlowRepository.InsertIncomeStatementDeductions(deductions);
        }
    }
}