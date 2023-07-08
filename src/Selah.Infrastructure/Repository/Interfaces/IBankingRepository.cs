using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Selah.Domain.Data.Models.Banking;

namespace Selah.Infrastructure.Repository.Interfaces
{
    public interface IBankingRepository
    {
        /// <summary>
        /// Inserts all bank accounts on institution link
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        public Task<int> ImportAccounts(List<BankAccount> accounts);

        /// <summary>
        /// Returns all accounts by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<IEnumerable<BankAccount>> GetAccounts(int userId, int limit, int offset);

        /// <summary>
        /// Returns a list of accounts by institution
        /// </summary>
        /// <param name="institutionId"></param>
        /// <returns></returns>
        public Task<IEnumerable<BankAccount>> GetAccountsByInstitutionId(Guid institutionId);

        /// <summary>
        /// Returns a bank account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<BankAccount> GetAccountById(int id);
    }
}