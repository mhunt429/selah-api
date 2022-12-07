using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Selah.Domain.Data.Models.Transactions;
using Selah.Domain.Data.Models.Transactions.Sql;

namespace Selah.Application.Services.Interfaces
{
    public interface ITransactionService
    {
        /// <summary>
        /// Imports transactions in batch from a Plaid data import
        /// </summary>
        /// <param name="institutionId">The Id of the financial institution to import transactions from </param>
        /// <returns></returns>
        public Task ImportTransactions(Guid institutionId);

        public Task<IEnumerable<UserTransaction>> GetTransactions();

        public Task<List<UserTransactionResponse>> GetTransactions(Guid userId, int take);

        public Task<UserTransactionCategory> CreateTransactionCategory(UserTransactionCategoryCreate category);

        public Task<IEnumerable<UserTransactionCategory>> GetTransactionCategoriesByUser(Guid userId);

        public Task<UserTransaction> CreateTransaction(UserTransaction transaction);

        public Task<IEnumerable<ItemizedTransactionSql>> GetItemizedTransactionAsync(Guid transactionId);
    }
}