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
        Task<IEnumerable<UserTransactionCategory>> GetTransactionCategoriesByUser(long userId);

        Task<long> InsertTransaction(TransactionCreateSql transaction);

        /// <summary>
        /// Split transactions is the business case for this query
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns>1 element per transaction line item</returns>
        Task<IEnumerable<ItemizedTransactionSql>> GetItemizedTransactionAsync(long transactionId);

        Task<int> InsertTransactionLineItems(IReadOnlyCollection<TransactionLineItemCreate> items);

        /// <summary>
        /// Returns a list of most recent transactions. 
        /// Limit param is a user setting
        /// </summary>F
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<IEnumerable<RecentTransactionSql>> GetRecentTransactions(long userId);

        /// <summary>
        /// Returns a list of transaction date, total amount, and count of transactions within a given date range
        /// date range, and the second item is the total
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<IEnumerable<TransactionSummarySql>> GetTransactionSummaryByDateRange(long userId, DateTime startDate,
            DateTime endDate);

        /// <summary>
        /// returns a transactio is by user and name
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="catgoryName">name of the category</param>
        /// <returns></returns>
        Task<IEnumerable<UserTransactionCategory>>
            GetTransactionCategoriesByUserAndName(long userId, string catgoryName);

        Task<IEnumerable<UserTransactionCategory>> GetTransactionCategoryById(long userId, long id);


        Task<long> CreateTransactionCategory(UserTransactionCategoryCreate category);

        /// <summary>
        /// Gets the total for transaction amounts by categories filtered by a user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<TransactionAmountByCategorySql>> GetTransactionTotalsByCategory(long userId);

        Task<TransactionCategoryDetailSql> GetTransactionCategoryDetails(long userId, long categoryId, DateTime startDate,
            DateTime endDate);
    }
}