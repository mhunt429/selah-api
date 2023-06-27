using Moq;
using Selah.Application.Queries.Banking;
using Selah.Domain.Data.Models.Banking;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;

namespace Selah.Application.UnitTests.Banking;

public class GetAllBankAccountsQueryTests
{
    private readonly Mock<IBankingRepository> _bankingRepositoryMock;
    private readonly GetAllBankAccountsQuery.Handler _handler;

    public GetAllBankAccountsQueryTests()
    {
        _bankingRepositoryMock = new Mock<IBankingRepository>();
        _handler = new GetAllBankAccountsQuery.Handler(_bankingRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsBankAccounts()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var bankAccounts = new List<BankAccount>
        {
            new() { Id = Guid.NewGuid(), UserId = userId, Name = "Checking" },
            new() { Id = Guid.NewGuid(), UserId = userId, Name = "Savings" }
        };
        _bankingRepositoryMock.Setup(x => x.GetAccounts(userId, It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(bankAccounts);

        var query = new GetAllBankAccountsQuery { UserId = userId, Limit = 10, Offset = 0 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(bankAccounts, result);
        _bankingRepositoryMock.Verify(x => x.GetAccounts(userId, query.Limit, query.Offset), Times.Once);
    }

    [Fact]
    public async Task Handle_ThrowsException_WhenRepositoryThrowsException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _bankingRepositoryMock.Setup(x => x.GetAccounts(userId, It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new Exception());

        var query = new GetAllBankAccountsQuery { UserId = userId, Limit = 10, Offset = 0 };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
        _bankingRepositoryMock.Verify(x => x.GetAccounts(userId, query.Limit, query.Offset), Times.Once);
    }
}