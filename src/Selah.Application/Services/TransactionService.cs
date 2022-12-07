using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.Banking;
using Selah.Domain.Data.Models.Plaid;
using Selah.Domain.Data.Models.Plaid.PlaidTransactions;
using Selah.Domain.Data.Models.Transactions;
using Selah.Domain.Data.Models.Transactions.Sql;
using Selah.Domain.Internal;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IPlaidService _plaidService;
        private readonly ISecurityService _securityService;
        private ILogger _logger;
        private readonly IOptions<EnvVariablesConfig> _envVariables;
        private readonly IUserInstitutionRepository _institutionRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IBankingRepository _bankingRepository;
        public TransactionService(IPlaidService plaidService,
          IUserInstitutionRepository institutionRepository,
          ISecurityService securityService,
          ILogger<TransactionService> logger,
          IOptions<EnvVariablesConfig> envVariables,
          ITransactionRepository transactionRepository,
          IBankingRepository bankingRepository)
        {
            _plaidService = plaidService;
            _securityService = securityService;
            _logger = logger;
            _envVariables = envVariables;
            _institutionRepository = institutionRepository;
            _transactionRepository = transactionRepository;
            _bankingRepository = bankingRepository;
        }

        /// <summary>
        /// Imports a list of transactions from Plaid for a given user based on the institution id
        /// Max is 500 transactions so 
        /// </summary>
        /// <param name="institutionId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task ImportTransactions(Guid institutionId)
        {
            UserInstitution userInstitution = await _institutionRepository.GetById(institutionId);
            string decryptedAccessToken = await _securityService.Decrypt(userInstitution.EncryptedAccessToken);
            IEnumerable<BankAccount> accounts = await _bankingRepository.GetAccountsByInstitutionId(institutionId);
            IEnumerable<UserTransactionCategory> existingcategories = await _transactionRepository.GetTransactionCategoriesByUser(userInstitution.UserId);

            var transactionsRequest = new PlaidTransactionRequest
            {
                AccessToken = decryptedAccessToken,
                ClientId = _envVariables.Value.PlaidClientId,
                Secret = _envVariables.Value.PlaidClientSecret,
                Options = new PlaidTransactionRequestOptions { Count = 500 },
                StartDate = "2021-01-01",
                EndDate = "2021-12-31"
            };
            _logger.LogInformation($"Importing {userInstitution.InstitutionName} transactions for user {userInstitution.UserId}");
            try
            {
                var importedTransactions = await _plaidService.ImportTransactions(transactionsRequest);
                var transactions = new List<UserTransaction>();

                //In the event that multiple transactions are imported with the same category and that category hasn't previously been imported
                var categoriesToImport = new HashSet<string>();

                foreach (var transaction in importedTransactions.Transactions)
                {
                    transactions.Add(new UserTransaction
                    {
                        Id = Guid.NewGuid(),
                        UserId = userInstitution.UserId,
                        AccountId = accounts.Where(a => a.ExternalAccountId == transaction.AccountId).Select(a => a.Id).FirstOrDefault(),
                        MerchantName = transaction.MerchantName,
                        Pending = transaction.Pending,
                        TransactionDate = transaction.Date,
                        PaymentMethod = transaction.PaymentMeta?.PaymentMethod,
                        TransactionAmount = transaction.Amount,
                        TransactionName = transaction.Name,
                        ExternalTransactionId = transaction.TransactionId
                    }); ;

                    //This is brute force and I don't really like it but it's the best thing I can thing of 
                    foreach (string item in transaction.Category)
                    {
                        categoriesToImport.Add(item);
                    }
                }

                foreach (string item in categoriesToImport)
                {
                    if (!existingcategories.Select(ec => ec.CategoryName).Contains(item))
                    {
                        await _transactionRepository.CreateTransactionCategory(new UserTransactionCategoryCreate { CategoryName = item, UserId = userInstitution.UserId });
                    }
                }

                //Insert the transactions for a user and use the response to create a new line item transaction with an empty category 
                List<Guid> insertedIds = await _transactionRepository.InsertTransactions(transactions);

                foreach (Guid id in insertedIds)
                {
                    await _transactionRepository.InsertTransactionLineItem(new TransactionLineItemCreate
                    {
                        TransactionId = id,
                        TransactionCategoryId = null,
                        ItemizedAmount = transactions.Where(x => x.Id == id).Select(x => x.TransactionAmount).FirstOrDefault()
                    });
                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"Failed to import transactions with error: {errorText(ex)}");
                throw new Exception("An error occurred importing transactions");
            }
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
            if (insertedId != null)
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

        public async Task<UserTransaction> CreateTransaction(UserTransaction transaction)
        {
            transaction.TransactionDate = DateTime.UtcNow;
            var transactionId = await _transactionRepository.InsertTransaction(transaction);
            transaction.Id = transactionId;
            return transaction;
        }
        private string errorText(Exception ex)
        {
            return $"\nError: {ex.Source}\nMessage: {ex.Message}\nStack: {ex.StackTrace}";
        }

        public async Task<IEnumerable<ItemizedTransactionSql>> GetItemizedTransactionAsync(Guid transactionId)
        {
            return await _transactionRepository.GetItemizedTransactionAsync(transactionId);
        }
    }
}