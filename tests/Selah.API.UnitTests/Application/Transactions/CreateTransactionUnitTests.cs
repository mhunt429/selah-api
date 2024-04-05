using FluentAssertions;
using FluentAssertions.LanguageExt;
using NSubstitute;
using Selah.Application.Commands.Transactions;
using Selah.Application.Services.Interfaces;
using Selah.Application.UnitTests.Transactions.TestHelpers;
using Selah.Domain.Data.Models.Banking;
using Selah.Domain.Data.Models.Transactions;
using Selah.Infrastructure.Repository.Interfaces;
using Selah.Infrastructure.Services.Validators;
using Xunit;

namespace Selah.Application.UnitTests.Application.Transactions;

public class CreateTransactionUnitTests
{
    private readonly IBankingRepository _bankingRepoMock = Substitute.For<IBankingRepository>();
    private readonly ITransactionRepository _transactionRepoMock = Substitute.For<ITransactionRepository>();
    private readonly ISecurityService _securityService = Substitute.For<ISecurityService>();

    private readonly ITransactionValidatorService _transactionValidatorService;

    public CreateTransactionUnitTests()
    {
        _transactionRepoMock.GetTransactionCategoryById(Arg.Any<long>(), Arg.Any<long>())
            .Returns(TransactionCategoryTestHelpers.CreateCategories(true));

        _bankingRepoMock.GetAccountById(Arg.Any<long>()).Returns((BankAccountSql)null);

        _securityService.DecodeHashId(Arg.Any<string>()).Returns(1);

        _transactionValidatorService =
            new TransactionValidatorService(_transactionRepoMock, _bankingRepoMock, _securityService);
    }


    [Fact]
    public async Task Can_Save_Valid_Model()
    {
        _transactionRepoMock.GetTransactionCategoryById(Arg.Any<long>(), Arg.Any<long>())
            .Returns(TransactionCategoryTestHelpers.CreateCategories());

        _transactionRepoMock.InsertTransaction(Arg.Any<TransactionCreateSql>()).Returns(1);

        _bankingRepoMock.GetAccountById(Arg.Any<long>()).Returns(new BankAccountSql());

        //Arrange
        var command = new CreateTransactionCommand
        {
            Data = new TransactionCreate
            {
                UserId = "abc123",
                AccountId = "accountId",
                TransactionAmount = 100,
                TransactionDate = DateTime.UtcNow.AddDays(-1),
                MerchantName = "Test Vendor",
                LineItems = TransactionCategoryTestHelpers.CreateLineItems()
            }
        };

        var handler =
            new CreateTransactionCommand.Handler(_transactionRepoMock, _securityService, _transactionValidatorService);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.Should().BeRight(x => x.TransactionId.Should().Be(1));

        result.Should().BeRight(x => x.TransactionDate.Should().Be(command.Data.TransactionDate));
        result.Should().BeRight(x => x.LineItems.Should().Be(4));
        result.Should().BeRight(x =>
            x.TranscationAmount.Should().Be(command.Data.LineItems.Sum(x => x.ItemizedAmount)));
    }
}