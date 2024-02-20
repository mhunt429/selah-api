using System.Collections.Generic;
using System.Threading.Tasks;
using Selah.Domain.Data.Models.Banking;

namespace Selah.Infrastructure.Repository.Interfaces
{
    public interface IBankingRepository
    {
        /// <summary>
        /// Returns all accounts by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<IEnumerable<BankAccountSql>> GetAccounts(long userId, int limit, int offset);

        /// <summary>
        /// Returns a list of accounts by institution
        /// </summary>
        /// <param name="institutionId"></param>
        /// <returns></returns>
        public Task<IEnumerable<BankAccountSql>> GetAccountsByInstitutionId(long institutionId);

        /// <summary>
        /// Returns a bank account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<BankAccountSql> GetAccountById(long id);
    }
}