using Selah.Domain.Data.Models.Analytics.Dashboard;
using Selah.Domain.Data.Models.Transactions;
using Selah.Domain.Data.Models.Transactions.Sql;
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
        private readonly IBaseRepository _baseRepository;

        public TransactionRepository(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }


        public async Task<int> CreateTransactionCategory(UserTransactionCategoryCreate category)
        {
            string sql = @"INSERT INTO user_transaction_category(user_id, name) 
               VALUES(@user_id, @name);
               SELECT LAST_INSERT_ID();";

            var parameters = new
            {
                user_id = category.UserId,
                name = category.Name
            };
            return await _baseRepository.AddAsync<int>(sql, parameters);
        }

        public async Task<IEnumerable<UserTransactionCategory>> GetTransactionCategoriesByUser(int userId)
        {
            string sql = @"SELECT id, user_id, name 
            FROM user_transaction_category WHERE user_id = @user_id";
            var parameters = new
            {
                user_id = userId
            };
            return await _baseRepository.GetAllAsync<UserTransactionCategory>(sql, parameters);
        }

        public async Task<IEnumerable<UserTransactionCategory>> GetTransactionCategoryById(int userId, int id)
        {
            string sql = @"SELECT id, user_id, name 
            FROM user_transaction_category WHERE user_id = @user_id AND id = id";
            var parameters = new
            {
                user_id = userId,
                id
            };
            return await _baseRepository.GetAllAsync<UserTransactionCategory>(sql, parameters);
        }

        public async Task<IEnumerable<UserTransactionCategory>> GetTransactionCategoriesByUserAndName(int userId,
            string catgoryName)
        {
            string sql = @"SELECT id, user_id, name 
            FROM user_transaction_category WHERE user_id = @user_id AND name = @name";
            var parameters = new
            {
                user_id = userId,
                name = catgoryName
            };
            return await _baseRepository.GetAllAsync<UserTransactionCategory>(sql, parameters);
        }

        public async Task<int> InsertTransaction(TransactionCreate transaction)
        {
            string sql = @"
    INSERT INTO user_transaction(
        account_id, user_id, transaction_amount,
        transaction_date, merchant_name, transaction_name, pending, payment_method, recurring_transaction_id
    )
    VALUES (
        @account_id, @user_id, @transaction_amount,
        @transaction_date, @merchant_name, @transaction_name, @pending, @payment_method, @recurring_transaction_id
    );
    SELECT LAST_INSERT_ID();";
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
                recurring_transaction_id = transaction.RecurringTransactionId
            };
            return await _baseRepository.AddAsync<int>(sql, objectToSave);
        }

        public async Task<IEnumerable<ItemizedTransactionSql>> GetItemizedTransactionAsync(int transactionId)
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


        public async Task<IEnumerable<TransactionAmountByCategorySql>> GetTransactionTotalsByCategory(int userId)
        {
            string sql = @"
    SELECT utc.id, SUM(tli.itemized_amount) AS total_itemized_amount,
           utc.name
    FROM transaction_line_item tli
    INNER JOIN user_transaction_category utc ON tli.transaction_category_id = utc.id
    WHERE tli.transaction_id IN (SELECT id FROM user_transaction WHERE user_id = @user_id)
    GROUP BY utc.id, utc.name;
";

            var parameters = new { user_id = userId };

            return await _baseRepository.GetAllAsync<TransactionAmountByCategorySql>(sql, parameters);
        }

        public async Task<TransactionCategoryDetailSql> GetTransactionCategoryDetails(int userId, int categoryId,
            DateTime startDate, DateTime endDate)
        {
            string sql = @"
                SELECT utc.id, 
                       utc.name,  
                       COUNT(1) as total,
                        COALESCE(SUM(itemized_amount), 0) as transactions,
                FROM transaction_line_item
                INNER JOIN 
                    user_transaction_category utc on transaction_line_item.transaction_category_id = utc.id
                WHERE 
                    transaction_category_id = @category_id
                  AND user_id = @user_id
                AND transaction_id IN
                (SELECT id 
                 FROM user_transaction
                Where 
                    user_id = @user_id
                  AND transaction_date BETWEEN  @start_date AND @end_date)
            ";
            var parameters = new
            {
                user_id = userId,
                category_id = categoryId,
                start_date = startDate,
                end_date = endDate
            };

            return await _baseRepository.GetFirstOrDefaultAsync<TransactionCategoryDetailSql>(sql, parameters);
        }
    }
}