using Selah.Domain.Data.Models.CashFlow;
using Selah.Domain.Reflection;
using Selah.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var id = _baseRepository.AddAsync<long>(sql, ObjectReflection.ConvertToSnakecase(incomeStatement));
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
                INSERT INTO income_statement_deduction (statement_id, deduction_name, amount)
                VALUES(@statement_id, @deduction_name, @amount)";
            var parmaters = deductions.Select(x => ObjectReflection.ConvertToSnakecase(x)).ToList();
            return await _baseRepository.AddManyAsync<IncomeStatementDeduction>(sql, parmaters);
        }

        public async Task<IEnumerable<IncomeStatementDeduction>> GetDeductionsByStatement(long id, Guid userId)
        {
            string sql = @"SELECT isd.id as id, isd.statement_id as statement_id, deduction_name, amount FROM income_statement_deduction isd 
						INNER JOIN income_statement inc_st ON isd.statement_id = inc_st.id 
						AND inc_st.id = @id
						AND inc_st.user_id =  @user_id";
            var parameters = new { id, user_id = userId };
            var deductions = await _baseRepository.GetAllAsync<IncomeStatementDeduction>(sql, parameters);
            return deductions;
        }
    }
}
