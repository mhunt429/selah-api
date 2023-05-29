using Selah.Domain.Data.Models.CashFlow;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Selah.Infrastructure.Repository.Interfaces
{
    public interface ICashFlowRepository
    {
        public Task<long> CreateIncomeStatement(IncomeStatementCreate incomeStatement);
        public Task<int> InsertIncomeStatementDeductions(IReadOnlyCollection<IncomeStatementDeduction> deductions);
        public Task<IEnumerable<IncomeStatement>> GetIncomeStatementsByUser(Guid userId, int limit, int offset);

        public Task<IEnumerable<IncomeStatementDeduction>> GetDeductionsByStatement(long id, Guid userId);
    }
}
