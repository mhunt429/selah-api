using Selah.Domain.Data.Models.CashFlow;
using Selah.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Selah.Infrastructure.Repository
{
    public class CashFlowRepository : ICashFlowRepository
    {
        private readonly IBaseRepository _baseRepository;
        public CashFlowRepository(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public Task<long> CreateIncomeStatement(IncomeStatementCreate incomeStatement)
        {
            string sql = @"
                INSERT INTO income_statement(user_id, statement_start_date, statement_end_date,total_pay) 
                VALUES(@user_id, @statement_start_date, @statement_end_date, @total_pay) returning(id)";
            var parameters = new
            {
                user_id = incomeStatement.UserId,
                statement_start_date = incomeStatement.StartDate,
                statement_end_date = incomeStatement.EndDate,
                total_pay = incomeStatement.TotalPay
            };
            var id = _baseRepository.AddAsync<long>(sql, parameters);
            return id;
        }

        public async Task<IEnumerable<IncomeStatement>> GetIncomeStatementsByUser(Guid userId, int limit, int offset)
        {
            string sql = @"SELECT * 
                FROM 
                income_statement 
                WHERE user_id = @user_id 
                LIMIT @limit OFSSET @offset";

            return await _baseRepository.GetAllAsync<IncomeStatement>(sql, new { limit, offset });
        }

        /// <summary>
        /// Returns the total number of rows inserted
        /// </summary>
        /// <param name="deductions"></param>
        /// <returns></returns>
        public async Task<int> InsertIncomeStatementDeductions(IReadOnlyCollection<IncomeStatementDeduction> deductions)
        {
            string sql = @"
                INSERT INTO income_statement_deduction (income_statement_id, deduction_name, amount)
                VALUES(@income_statement_id, @deduction_name, @amount)";

            return await _baseRepository.AddManyAsync<IncomeStatementDeduction>(sql, deductions);
        }
    }
}
