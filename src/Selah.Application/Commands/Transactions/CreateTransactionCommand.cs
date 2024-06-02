using MediatR;
using Selah.Domain.Data.Models.Transactions;
using Selah.Domain.Data.Models.Transactions.Commands;
using Selah.Infrastructure.Repository.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using FluentValidation.Results;
using LanguageExt;
using Selah.Application.Services.Interfaces;
using Selah.Infrastructure.Services.Validators;

namespace Selah.Application.Commands.Transactions
{
    public class CreateTransactionCommand : IRequest<Either<ValidationResult, TransactionCreateResponse>>
    {
        public TransactionCreate Data { get; set; } = new TransactionCreate();

        public class Handler : IRequestHandler<CreateTransactionCommand,
            Either<ValidationResult, TransactionCreateResponse>>
        {
            private readonly ITransactionRepository _transactionRepository;
            private readonly ISecurityService _securityService;
            private readonly ITransactionValidatorService _transactionValidatorService;

            public Handler(ITransactionRepository transactionRepository, ISecurityService securityService,
                ITransactionValidatorService transactionValidatorService)
            {
                _transactionRepository = transactionRepository;
                _securityService = securityService;
                _transactionValidatorService = transactionValidatorService;
            }


            public async Task<Either<ValidationResult, TransactionCreateResponse>> Handle(
                CreateTransactionCommand command,
                CancellationToken cancellationToken)
            {
                ValidationResult validationResult = await _transactionValidatorService.ValidateAsync(command.Data);
                if (!validationResult.IsValid)
                {
                    return Either<ValidationResult, TransactionCreateResponse>.Left(validationResult);
                }

                long transactionId = await _transactionRepository.InsertTransaction(MapToDbCreateModel(command));

                await _transactionRepository.InsertTransactionLineItems(command.Data.LineItems.Select(x =>
                        new TransactionLineItemCreate
                        {
                            TransactionId = transactionId,
                            DefaultCategory = x.DefaultCategory,
                            ItemizedAmount = x.ItemizedAmount,
                            TransactionCategoryId = x.TransactionCategoryId
                        })
                    .ToList()
                );

                return Either<ValidationResult, TransactionCreateResponse>.Right(new TransactionCreateResponse
                {
                    TransactionId = transactionId,
                    TransactionDate = command.Data.TransactionDate,
                    TranscationAmount = command.Data.TransactionAmount,
                    LineItems = command.Data.LineItems.Count
                });
            }

            private TransactionCreateSql MapToDbCreateModel(CreateTransactionCommand command)
            {
                var transaction = new TransactionCreateSql
                {
                    UserId = _securityService.DecodeHashId(command.Data.UserId),
                    AccountId = _securityService.DecodeHashId(command.Data.AccountId),
                    TransactionAmount = command.Data.TransactionAmount,
                    TransactionDate = command.Data.TransactionDate,
                    MerchantName = command.Data.MerchantName,
                    TransactionName = command.Data.TransactionName,
                    PaymentMethod = command.Data.PaymentMethod,
                    Pending = command.Data.Pending
                };

                if (!string.IsNullOrEmpty(command.Data.RecurringTransactionId))
                {
                    transaction.RecurringTransactionId =
                        _securityService.DecodeHashId(command.Data.RecurringTransactionId);
                }

                return transaction;
            }
        }
    }
}