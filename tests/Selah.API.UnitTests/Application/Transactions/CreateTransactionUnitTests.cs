using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using Selah.Application.Commands.Transactions;
using Selah.Application.Services.Interfaces;
using Selah.Application.UnitTests.Transactions.TestHelpers;
using Selah.Domain.Data.Models.Banking;
using Selah.Domain.Data.Models.Transactions;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;

namespace Selah.Application.UnitTests.Transactions;

public class CreateTransactionUnitTests
{
    private readonly IBankingRepository _bankingRepoMock = Substitute.For<IBankingRepository>();
    private readonly ITransactionRepository _transactionRepoMock = Substitute.For<ITransactionRepository>();
    private readonly ISecurityService _securityService = Substitute.For<ISecurityService>();

    public CreateTransactionUnitTests()
    {
        _transactionRepoMock.GetTransactionCategoryById(Arg.Any<long>(), Arg.Any<long>())
            .Returns(TransactionCategoryTestHelpers.CreateCategories(true));

        _bankingRepoMock.GetAccountById(Arg.Any<long>()).Returns((BankAccountSql)null);

        _securityService.DecodeHashId(Arg.Any<string>()).Returns(1);
    }

    [Fact]
    public async Task Can_Validate_Against_Invalid_Model()
    {
        //Arrange
        var command = new CreateTransactionCommand
        {
            AccountId = "accountId",
            TransactionAmount = 0,
            TransactionDate = DateTime.UtcNow.AddDays(1),
            MerchantName = "",
            LineItems = TransactionCategoryTestHelpers.CreateLineItems()
        };

        var validator =
            new CreateTransactionCommand.Validator(_transactionRepoMock, _bankingRepoMock, _securityService);

        //Act
        var result = await validator.TestValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.TransactionDate);
        result.ShouldHaveValidationErrorFor(x => x.MerchantName);
        result.ShouldHaveValidationErrorFor(x => x.TransactionAmount);
        result.ShouldHaveValidationErrorFor(x => x.AccountId);
    }

    [Fact]
    public async Task Can_Save_Valid_Model()
    {
        _transactionRepoMock.GetTransactionCategoryById(Arg.Any<long>(), Arg.Any<long>())
            .Returns(TransactionCategoryTestHelpers.CreateCategories());

        _transactionRepoMock.InsertTransaction(Arg.Any<TransactionCreate>()).Returns(1);

        _bankingRepoMock.GetAccountById(Arg.Any<long>()).Returns(new BankAccountSql());

        //Arrange
        var command = new CreateTransactionCommand
        {
            AccountId = "accountId",
            TransactionAmount = 100,
            TransactionDate = DateTime.UtcNow.AddDays(-1),
            MerchantName = "Test Vendor",
            LineItems = TransactionCategoryTestHelpers.CreateLineItems()
        };

        var validator =
            new CreateTransactionCommand.Validator(_transactionRepoMock, _bankingRepoMock, _securityService);

        var handler = new CreateTransactionCommand.Handler(_transactionRepoMock, _securityService);

        //Act
        var validationResult = await validator.TestValidateAsync(command);
        validationResult.IsValid.Should().BeTrue();

        var result = handler.Handle(command, CancellationToken.None);

        //Assert
        result.Result.Should().NotBeNull();
        result.Result.TransactionId.Should().Be(1);
        result.Result.TransactionDate.Should().Be(command.TransactionDate);
        result.Result.LineItems.Should().Be(4);
        result.Result.TranscationAmount.Should().Be(command.LineItems.Sum(x => x.ItemizedAmount));
    }
}