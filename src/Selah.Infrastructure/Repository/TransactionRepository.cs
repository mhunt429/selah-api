using Dapper;
using Microsoft.AspNetCore.Mvc.ModelBinding.Internal;
using Microsoft.Extensions.Options;
using Npgsql;
using Selah.Domain.Data.Models.Analytics.Dashboard;
using Selah.Domain.Data.Models.Transactions;
using Selah.Domain.Data.Models.Transactions.Sql;
using Selah.Domain.Internal;
using Selah.Domain.Reflection;
using Selah.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Selah.Infrastructure.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IOptions<EnvVariablesConfig> _envVariables;
        private readonly IBaseRepository _baseRepository;

        public TransactionRepository(IOptions<EnvVariablesConfig> envVariables, IBaseRepository baseRepository)
        {
            _envVariables = envVariables;
            _baseRepository = baseRepository;
        }


        public async Task<int> CreateTransactionCategory(UserTransactionCategoryCreate category)
        {
            using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
            {
                return await connection.ExecuteScalarAsync<int>(@"INSERT INTO 
          user_transaction_category(user_id, category_name) 
          VALUES(@user_id, @category_name) RETURNING(id)", new
                {
                    user_id = category.UserId,
                    category_name = category.CategoryName
                });
            }
        }

        public async Task<IEnumerable<UserTransactionCategory>> GetTransactionCategoriesByUser(int userId)
        {
            using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
            {
                return await connection.QueryAsync<UserTransactionCategory>(@"SELECT id, user_id, category_name 
            FROM user_transaction_category WHERE user_id = @user_id", new
                {
                    user_id = userId
                });
            }
        }

        public async Task<IEnumerable<UserTransactionCategory>> GetTransactionCategoryById(int userId, int id)
        {
            using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
            {
                return await connection.QueryAsync<UserTransactionCategory>(@"SELECT id, user_id, category_name 
            FROM user_transaction_category WHERE user_id = @user_id AND id = id", new
                {
                    user_id = userId,
                    id
                });
            }
        }

        public async Task<IEnumerable<UserTransactionCategory>> GetTransactionCategoriesByUser(int userId,
            string catgoryName)
        {
            using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
            {
                return await connection.QueryAsync<UserTransactionCategory>(@"SELECT id, user_id, category_name 
            FROM user_transaction_category WHERE user_id = @user_id AND name = @name", new
                {
                    user_id = userId,
                    category_name = catgoryName
                });
            }
        }

        public async Task<int> InsertTransaction(TransactionCreate transaction)
        {
            string sql = @"
                INSERT INTO user_transaction(
                    account_id, user_id, transaction_amount,
                    transaction_date, merchant_name, transaction_name, pending, payment_method
                )
                VALUES (
                    @account_id, @user_id, @transaction_amount,
                    @transaction_date, @merchant_name, @transaction_name, @pending, @payment_method
                )
                RETURNING id";
            var objectToSave = new
            {
                account_id = transaction.AccountId,
                user_id = transaction.UserId,
                transaction_amount = transaction.TransactionAmount,
                transaction_date = transaction.TransactionDate,
                merchant_name = transaction.MerchantName,
                transaction_name = transaction.TransactionName,
                pending = transaction.Pending,
                payment_method = transaction.PaymentMethod,
            };
            return await _baseRepository.AddAsync<int>(sql, objectToSave);
        }

        public async Task<IEnumerable<ItemizedTransactionSql>> GetItemizedTransactionAsync(Guid transactionId)
        {
            var sql = "select * from get_transaction_line_items_by_transaction(@transaction_id)";

            var parameters = new
            {
                transaction_id = transactionId
            };

            return await _baseRepository.GetAllAsync<ItemizedTransactionSql>(sql, parameters);
        }

        public async Task<int> InsertTransactionLineItems(IReadOnlyCollection<TransactionLineItemCreate> items)
        {
            var sql = @"INSERT INTO transaction_line_item(transaction_id, transaction_category_id, itemized_amount) 
                        VALUES(@transaction_id, @transaction_category_id, @itemized_amount)";
            var parameters = items.Select(x => ObjectReflection.ConvertToSnakecase(x)).ToList();
            return await _baseRepository.AddManyAsync<TransactionLineItemCreate>(sql, parameters);
        }

        public async Task<IEnumerable<RecentTransactionSql>> GetRecentTransactions(int userId)
        {
            var sql = @"SELECT
                         ut.id as transaction_id,
                        transaction_date,
                        merchant_name,
                        transaction_amount,
                        account_name 
                        FROM
                            user_transaction ut
                        inner join user_bank_account uba on
                            ut.account_id = uba.id
                        where
                            ut.user_id = @user_id
                        ORDER BY 
                            transaction_date DESC
                        LIMIT 5";
            var parameters = new
            {
                user_id = userId
            };

            return await _baseRepository.GetAllAsync<RecentTransactionSql>(sql, parameters);
        }

        public async Task<IEnumerable<TransactionSummarySql>> GetTransactionSummaryByDateRange(int userId,
            DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT 
                        transaction_date, 
                        SUM(transaction_amount) AS total_amount, 
                        COUNT(*) as count
                    FROM user_transaction
                    WHERE 
                    user_id = @userId AND
                    transaction_date BETWEEN @startDate AND @endDate
                    GROUP BY transaction_date";

            var parameters = new { userId, startDate, endDate };
            return await _baseRepository.GetAllAsync<TransactionSummarySql>(sql, parameters);
        }

        /// <summary>
        /// For analytics purposes we want to show the user zero amounts so we do a generate series to get all dates
        /// within a given date range. The keeps us from having to add conditional logic for zero amounts. 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TransactionSummarySql>> GetEmptyTransactionSummary(DateTime startDate,
            DateTime endDate)
        {
            var sql =
                @"select generate_series(@start_date, @end_date, '1 day'::interval) AS transaction_date, 0.00 AS daily_total, 0 AS count";
            var parameters = new { start_date = startDate, end_date = endDate };
            return await _baseRepository.GetAllAsync<TransactionSummarySql>(sql, parameters);
        }
    }
}