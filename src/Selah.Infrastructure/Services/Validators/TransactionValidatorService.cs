using System.Linq;
using FluentValidation;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.Transactions;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Infrastructure.Services.Validators;

public interface ITransactionValidatorService : IValidator<TransactionCreate>
{
}

public class TransactionValidatorService : AbstractValidator<TransactionCreate>, ITransactionValidatorService
{
    private readonly ITransactionRepository _trxRepo;
    private readonly IBankingRepository _bankingRepo;
    private readonly ISecurityService _securityService;

    public TransactionValidatorService(ITransactionRepository trxRepo, IBankingRepository bankingRepo,
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
            }).WithMessage("The account associated with this transaction could not be found")
            .When(x => !string.IsNullOrEmpty(x.AccountId));
    }
}