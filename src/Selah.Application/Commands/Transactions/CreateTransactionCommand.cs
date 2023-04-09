﻿using FluentValidation;
using MediatR;
using Selah.Domain.Data.Models.Transactions;
using Selah.Domain.Data.Models.Transactions.Commands;
using Selah.Infrastructure.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
namespace Selah.Application.Commands.Transactions
{
    public class CreateTransactionCommand : IRequest<TransactionCreateResponse>
    {
        public TransactionCreate Data { get; set; } = new TransactionCreate();

        public class Validator : AbstractValidator<CreateTransactionCommand>
        {
            private readonly ITransactionRepository _trxRepo;
            private readonly IBankingRepository _bankingRepo;
            public Validator(ITransactionRepository trxRepo, IBankingRepository bankingRepo)
            {
                _trxRepo = trxRepo;
                _bankingRepo = bankingRepo;

                RuleFor(x => x.Data.MerchantName).NotEmpty()
                    .WithMessage("Merchant name cannot be empty");

                RuleFor(x => x.Data.TransactionAmount).GreaterThan(0);

                RuleFor(x => x.Data.TransactionDate).LessThanOrEqualTo(System.DateTime.UtcNow)
                    .WithMessage("Transaction date cannot be in the future");

                RuleFor(x => x.Data.LineItems.Sum(x => x.ItemizedAmount)).Equal(x => x.Data.TransactionAmount)
                    .WithMessage("The total of all line items must equal the transaction amount");

                RuleForEach(x => x.Data.LineItems).ChildRules(item =>
                {
                    item.RuleFor(x => x.TransactionCategoryId).MustAsync(async (model, id, cancellation) =>
                    {
                        var categories = await _trxRepo.GetTransactionCategoryById(model.UserId, id);
                        return categories.Any();
                    }).WithMessage("The category associated with this line item could not be found");
                });

                RuleFor(x => x.Data.AccountId).MustAsync(async (id, cancellation) =>
                {
                    var account = await _bankingRepo.GetAccountById(id);

                    return account != null;
                }).WithMessage("The account associated with this transaction could not be found");
            }
        }

        public class Handler : IRequestHandler<CreateTransactionCommand, TransactionCreateResponse>
        {
            private readonly ITransactionRepository _transactionRepository;

            public Handler(ITransactionRepository transactionRepository)
            {
                _transactionRepository = transactionRepository;
            }

            
            public async Task<TransactionCreateResponse> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
            {
                var transaction = command.Data;
                var transactionId = await _transactionRepository.InsertTransaction(transaction);

                if (transactionId > 0)
                {
                    var lineTitems = new List<TransactionLineItemCreate>();
                    foreach (var item in transaction.LineItems)
                    {
                        lineTitems.Add(new TransactionLineItemCreate
                        {
                            TransactionId = transactionId,
                            DefaultCategory = item.DefaultCategory,
                            ItemizedAmount = item.ItemizedAmount,
                            TransactionCategoryId = item.TransactionCategoryId
                        });
                    }
                    await CreateSplitTransactions(lineTitems, cancellationToken);

                    return new TransactionCreateResponse
                    {
                        TransactionId = transactionId,
                        TransactionDate = transaction.TransactionDate,
                        TranscationAmount = transaction.TransactionAmount,
                        LineItems = transaction.LineItems.Count
                    };
                }
                return null;
            }


            /// <summary>
            /// Allows a user to create multiple line items per transactions i.e $90 bucks for groceries and $10 for dog food
            /// </summary>
            /// <param name="items">A collection of transaction line items</param>
            /// <returns>returns an empty task because we don't care about the return value</returns>
            private async Task CreateSplitTransactions(List<TransactionLineItemCreate> items, CancellationToken cancellationToken)
            {
                //Open a max of 3 concurrent connections to the database
                await Parallel.ForEachAsync(items, new ParallelOptions { MaxDegreeOfParallelism = 3 }, async (item, cancellationToken) =>
                {
                    await _transactionRepository.InsertTransactionLineItem(item);
                });
            }
        }
    }
}
