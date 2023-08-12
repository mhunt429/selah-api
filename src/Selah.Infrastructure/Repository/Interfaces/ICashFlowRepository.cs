using Selah.Domain.Data.Models.CashFlow;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Selah.Infrastructure.Repository.Interfaces
{
    public interface ICashFlowRepository
    {
        public Task<int> CreateIncomeStatement(IncomeStatementCreate incomeStatement);
        public Task<int> InsertIncomeStatementDeductions(IReadOnlyCollection<IncomeStatementDeduction> deductions);
        public Task<IEnumerable<IncomeStatement>> GetIncomeStatementsByUser(int userId, int limit, int offset);

        public Task<IEnumerable<IncomeStatementDeduction>> GetDeductionsByStatement(int id, int userId);
    }
}
