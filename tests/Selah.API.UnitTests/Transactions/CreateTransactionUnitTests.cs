using Xunit;
using FluentAssertions;
using Moq;
using Selah.Application.Commands.Transactions;
using Selah.Infrastructure.Repository.Interfaces;
using Selah.Application.UnitTests.Transactions.TestHelpers;
using FluentValidation.TestHelper;
using Selah.Domain.Data.Models.Transactions;


namespace Selah.Application.UnitTests.Transactions
{
    public class CreateTransactionUnitTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepoMock = new Mock<ITransactionRepository>();

        public CreateTransactionUnitTests()
        {
            _transactionRepoMock.Setup(x => x.GetTransactionCategoryById(It.IsAny<Guid>(), It.IsAny<long>())).ReturnsAsync(TransactionCategoryTestHelpers.CreateCategories(true));
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

            var validator = new CreateTransactionCommand.Validator(_transactionRepoMock.Object);

            //Act
            var result = await validator.TestValidateAsync(command);

            //Assert
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(x => x.Data.TransactionDate);
            result.ShouldHaveValidationErrorFor(x => x.Data.MerchantName);
            result.ShouldHaveValidationErrorFor(x => x.Data.TransactionAmount);
        }

        [Fact]
        public async Task Can_Save_Valid_Model()
        {
            _transactionRepoMock.Setup(x => x.GetTransactionCategoryById(It.IsAny<Guid>(), It.IsAny<long>())).ReturnsAsync(TransactionCategoryTestHelpers.CreateCategories(false));
            _transactionRepoMock.Setup(x => x.InsertTransaction(It.IsAny<TransactionCreate>())).ReturnsAsync(1);

            //Arrange
            var command = new CreateTransactionCommand
            {
                Data = new TransactionCreate
                {
                    AccountId = 1,
                    TransactionAmount = 123,
                    TransactionDate = DateTime.UtcNow.AddDays(-1),
                    MerchantName = "Test Vendor",
                    LineItems = TransactionCategoryTestHelpers.CreateLineItems()
                }
            };

            var validator = new CreateTransactionCommand.Validator(_transactionRepoMock.Object);

            var handler = new CreateTransactionCommand.Handler(_transactionRepoMock.Object);

            //Act
            var validationResult = await validator.TestValidateAsync(command);
            validationResult.IsValid.Should().BeTrue();

            var result = handler.Handle(command, CancellationToken.None);
            _transactionRepoMock.Verify(x => x.InsertTransaction(It.IsAny<TransactionCreate>()), Times.Exactly(1));
            _transactionRepoMock.Verify(x => x.InsertTransactionLineItem(It.IsAny<TransactionLineItemCreate>()), Times.Exactly(4));

            result.Result.Should().NotBeNull();
            result.Result.TransactionId.Should().Be(1);
            result.Result.TransactionDate.Should().Be(command.Data.TransactionDate);
            result.Result.LineItems.Should().Be(4);
            result.Result.TranscationAmount.Should().Be(123);
        }
    }
}
