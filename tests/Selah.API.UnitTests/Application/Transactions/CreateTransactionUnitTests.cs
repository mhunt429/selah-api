using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using Selah.Application.Commands.Transactions;
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

    public CreateTransactionUnitTests()
    {
        _transactionRepoMock.GetTransactionCategoryById(Arg.Any<int>(), Arg.Any<int>())
            .Returns(TransactionCategoryTestHelpers.CreateCategories(true));
        
        _bankingRepoMock.GetAccountById(Arg.Any<int>()).Returns((BankAccountSql)null);
    }

    [Fact]
    public async Task Can_Validate_Against_Invalid_Model()
    {
        //Arrange
        var command = new CreateTransactionCommand
        {
            Data = new TransactionCreate
            {
                AccountId = 1,
                TransactionAmount = 0,
                TransactionDate = DateTime.UtcNow.AddDays(1),
                MerchantName = "",
                LineItems = TransactionCategoryTestHelpers.CreateLineItems()
            }
        };

        var validator = new CreateTransactionCommand.Validator(_transactionRepoMock, _bankingRepoMock);

        //Act
        var result = await validator.TestValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Data.TransactionDate);
        result.ShouldHaveValidationErrorFor(x => x.Data.MerchantName);
        result.ShouldHaveValidationErrorFor(x => x.Data.TransactionAmount);
        result.ShouldHaveValidationErrorFor(x => x.Data.AccountId);
    }

    [Fact]
    public async Task Can_Save_Valid_Model()
    {
        _transactionRepoMock.GetTransactionCategoryById(Arg.Any<int>(), Arg.Any<int>())
            .Returns(TransactionCategoryTestHelpers.CreateCategories());
        
        _transactionRepoMock.InsertTransaction(Arg.Any<TransactionCreate>()).Returns(1);
        
        _bankingRepoMock.GetAccountById(Arg.Any<int>()).Returns(new BankAccountSql());

        //Arrange
        var command = new CreateTransactionCommand
        {
            Data = new TransactionCreate
            {
                AccountId = 1,
                TransactionAmount = 100,
                TransactionDate = DateTime.UtcNow.AddDays(-1),
                MerchantName = "Test Vendor",
                LineItems = TransactionCategoryTestHelpers.CreateLineItems()
            }
        };

        var validator = new CreateTransactionCommand.Validator(_transactionRepoMock, _bankingRepoMock);

        var handler = new CreateTransactionCommand.Handler(_transactionRepoMock);

        //Act
        var validationResult = await validator.TestValidateAsync(command);
        validationResult.IsValid.Should().BeTrue();

        var result = handler.Handle(command, CancellationToken.None);

        //Assert
        result.Result.Should().NotBeNull();
        result.Result.TransactionId.Should().Be(1);
        result.Result.TransactionDate.Should().Be(command.Data.TransactionDate);
        result.Result.LineItems.Should().Be(4);
        result.Result.TranscationAmount.Should().Be(command.Data.LineItems.Sum(x => x.ItemizedAmount));
    }
}