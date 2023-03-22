using MediatR;
using Selah.Domain.Data.Models.Transactions;
using Selah.Domain.Data.Models.Transactions.Commands;
using Selah.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Selah.Application.Commands.Transactions
{
    public class CreateTransactionCommand : IRequest<TransactionCreateResponse>
    {
        public TransactionCreate Transaction { get; set; }

        public class Handler : IRequestHandler<CreateTransactionCommand, TransactionCreateResponse>
        {
            private readonly ITransactionRepository _transactionRepository;

            public Handler(ITransactionRepository transactionRepository)
            {
                _transactionRepository = transactionRepository;
            }

            //TODO add validation to this
            public async Task<TransactionCreateResponse> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
            {
                var transaction = command.Transaction;
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
            private async Task CreateSplitTransactions(IReadOnlyCollection<TransactionLineItemCreate> items, CancellationToken cancellationToken)
            {
                await Parallel.ForEachAsync(items, async (item, cancellationToken) =>
                {
                    await _transactionRepository.InsertTransactionLineItem(item);
                });
            }
        }
    }
}
