using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Selah.Domain.Data.Models.Analytics.Dashboard;
using Selah.Domain.Data.Models.Transactions;
using Selah.Domain.Data.Models.Transactions.Sql;

namespace Selah.Infrastructure.Repository.Interfaces
{
    public interface ITransactionRepository
    {
        /// <summary>
        /// Return a key value pair of the transaction id and the total transaction amount
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        public Task<List<Guid>> InsertTransactions(List<UserTransaction> transactions);

        public Task<IEnumerable<UserTransaction>> GetTransactions();

        public Task<IEnumerable<UserTransactionQueryResult>> GetTransactionsVM(Guid userId, int take);

        public Task<Guid> CreateTransactionCategory(UserTransactionCategoryCreate category);

        public Task<IEnumerable<UserTransactionCategory>> GetTransactionCategoriesByUser(Guid userId);

        public Task<Guid> InsertTransaction(UserTransaction transaction);

        /// <summary>
        /// Split transactions is the business case for this query
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns>1 element per transaction line item</returns>
        public Task<IEnumerable<ItemizedTransactionSql>> GetItemizedTransactionAsync(Guid transactionId);

        public Task InsertTransactionLineItem(TransactionLineItemCreate lineItem);

        /// <summary>
        /// Returns a list of most recent transactions. 
        /// Limit param is a user setting
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Task<IEnumerable<RecentTransactionSql>> GetRecentTransactions(Guid userId);

        /// <summary>
        /// Returns a list of transaction date, total amount, and count of transactions within a given date range
        /// date range, and the second item is the total
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public Task<IEnumerable<TransactionSummarySql>> GetTransactionSummaryByDateRange(Guid userId, DateTime startDate, DateTime endDate);

    }
}