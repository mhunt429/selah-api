using System;
using FluentValidation;
using MediatR;
using Selah.Domain.Data.Models.Transactions;
using Selah.Domain.Data.Models.Transactions.Commands;
using Selah.Infrastructure.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Selah.Application.Services.Interfaces;

namespace Selah.Application.Commands.Transactions
{
    public class CreateTransactionCommand : IRequest<TransactionCreateResponse>
    {
        public string UserId { get; set; }

        public string AccountId { get; set; }

        public decimal TransactionAmount { get; set; }

        public DateTime TransactionDate { get; set; }

        public string MerchantName { get; set; }

        public string TransactionName { get; set; }

        public bool Pending { get; set; }

        public string PaymentMethod { get; set; }

        public string RecurringTransactionId { get; set; }
        public List<LineItem> LineItems { get; set; } = new List<LineItem>();


        public class Validator : AbstractValidator<CreateTransactionCommand>
        {
            private readonly ITransactionRepository _trxRepo;
            private readonly IBankingRepository _bankingRepo;
            private readonly ISecurityService _securityService;

            public Validator(ITransactionRepository trxRepo, IBankingRepository bankingRepo,
                ISecurityService securityService)
            {
                _trxRepo = trxRepo;
                _bankingRepo = bankingRepo;
                _securityService = securityService;
                RuleFor(x => x.MerchantName).NotEmpty()
                    .WithMessage("Merchant name cannot be empty");

                RuleFor(x => x.TransactionAmount).GreaterThan(0);

                RuleFor(x => x.TransactionDate).LessThanOrEqualTo(System.DateTime.UtcNow)
                    .WithMessage("Transaction date cannot be in the future");

                RuleFor(x => x.LineItems.Sum(x => x.ItemizedAmount)).Equal(x => x.TransactionAmount)
                    .WithMessage("The total of all line items must equal the transaction amount");

                RuleForEach(x => x.LineItems).ChildRules(item =>
                {
                    item.RuleFor(x => x.TransactionCategoryId).MustAsync(async (model, id, cancellation) =>
                    {
                        var categories = await _trxRepo.GetTransactionCategoryById(model.UserId, id);
                        return categories.Any();
                    }).WithMessage("The category associated with this line item could not be found");
                });
                RuleFor(x => x.AccountId).NotEmpty();
                
                RuleFor(x => x.AccountId).MustAsync(async (id, cancellation) =>
                {
                    long bankAccountId = _securityService.DecodeHashId(id);
                    var account = await _bankingRepo.GetAccountById(bankAccountId);

                    return account != null;
                }).WithMessage("The account associated with this transaction could not be found").When(x => !string.IsNullOrEmpty(x.AccountId));
            }
        }

        public class Handler : IRequestHandler<CreateTransactionCommand, TransactionCreateResponse>
        {
            private readonly ITransactionRepository _transactionRepository;
            private readonly ISecurityService _securityService;

            public Handler(ITransactionRepository transactionRepository, ISecurityService securityService)
            {
                _transactionRepository = transactionRepository;
                _securityService = securityService;
            }


            public async Task<TransactionCreateResponse> Handle(CreateTransactionCommand command,
                CancellationToken cancellationToken)
            {
                var transactionId = await _transactionRepository.InsertTransaction(MapToDbCreateModel(command));

                if (transactionId > 0)
                {
                    await _transactionRepository.InsertTransactionLineItems(command.LineItems.Select(x =>
                        new TransactionLineItemCreate
                        {
                            TransactionId = transactionId,
                            DefaultCategory = x.DefaultCategory,
                            ItemizedAmount = x.ItemizedAmount,
                            TransactionCategoryId = x.TransactionCategoryId
                        })
                        .ToList()
                    );

                    return new TransactionCreateResponse
                    {
                        TransactionId = transactionId,
                        TransactionDate = command.TransactionDate,
                        TranscationAmount = command.TransactionAmount,
                        LineItems = command.LineItems.Count
                    };
                }

                return null;
            }

            private TransactionCreate MapToDbCreateModel(CreateTransactionCommand command)
            {
                var transaction = new TransactionCreate
                {
                    UserId = _securityService.DecodeHashId(command.UserId),
                    AccountId = _securityService.DecodeHashId(command.AccountId),
                    TransactionAmount = command.TransactionAmount,
                    TransactionDate = command.TransactionDate,
                    MerchantName = command.MerchantName,
                    TransactionName = command.TransactionName,
                    PaymentMethod = command.PaymentMethod,
                    Pending = command.Pending
                };

                if (!string.IsNullOrEmpty(command.RecurringTransactionId))
                {
                    transaction.RecurringTransactionId = _securityService.DecodeHashId(command.RecurringTransactionId);
                }

                return transaction;
            }
        }
    }
}