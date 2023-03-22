using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.Transactions;
using Selah.Domain.Data.Models.Transactions.Commands;
using Selah.Domain.Data.Models.Transactions.Sql;
using Selah.Domain.Internal;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Application.Services
{
    public class TransactionService : ITransactionService
    {

        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(
          ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<UserTransaction>> GetTransactions()
        {
            return await _transactionRepository.GetTransactions();
        }

        public async Task<List<UserTransactionResponse>> GetTransactions(Guid userId, int take)
        {
            var transactionsResults = new List<UserTransactionResponse>();
            var transactions =
              (await _transactionRepository.GetTransactionsVM(userId, take)).GroupBy(trx => trx.TransactionDate);

            foreach (var item in transactions)
            {
                transactionsResults.Add(new UserTransactionResponse
                {
                    TotalTransactionAmount = item.Select(t => t.TransactionAmount).Sum(),
                    TransactionDate = item.Key.ToString("MM/dd/yyyy"),
                    Records = item.Where(x => x.TransactionDate == item.Key).Select(t => t.Records).FirstOrDefault(),
                    Transactions = item.Select(x => new UserTransactionVM
                    {
                        Id = x.Id,
                        TransactionDate = x.TransactionDate,
                        ExternalTransactionId = x.ExternalTransactionId,
                        TransactionAmount = x.TransactionAmount,
                        MerchantName = x.MerchantName,
                        TransactionName = x.TransactionName
                    }).Where(t => t.TransactionDate == item.Key).ToList()
                });
            }

            return transactionsResults;
        }

        public async Task<UserTransactionCategory> CreateTransactionCategory(UserTransactionCategoryCreate category)
        {
            var insertedId = await _transactionRepository.CreateTransactionCategory(category);
            if (insertedId > 0)
            {
                return new UserTransactionCategory
                {
                    Id = insertedId,
                    UserId = category.UserId,
                    CategoryName = category.CategoryName,
                };
            }

            return null;
        }

        public async Task<IEnumerable<UserTransactionCategory>> GetTransactionCategoriesByUser(Guid userId)
        {
            return await _transactionRepository.GetTransactionCategoriesByUser(userId);
        }

        public async Task<IEnumerable<ItemizedTransactionSql>> GetItemizedTransactionAsync(Guid transactionId)
        {
            return await _transactionRepository.GetItemizedTransactionAsync(transactionId);
        }
    }
}