using Moq;
using Selah.Application.Queries.Banking;
using Selah.Application.Services.Interfaces;
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
        Mock<ISecurityService> securityServiceMock = new();
        securityServiceMock.Setup(x => x.DecodeHashId(It.IsAny<string>())).Returns(1);
        securityServiceMock.Setup(x => x.EncodeHashId(It.IsAny<int>())).Returns("ABC123");
        _handler = new GetAllBankAccountsQuery.Handler(_bankingRepositoryMock.Object, securityServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsBankAccounts()
    {
        // Arrange
        var bankAccounts = new List<BankAccount>
        {
            new() { Id =1, UserId = 1, Name = "Checking" },
            new() { Id = 2, UserId = 1, Name = "Savings" }
        };
        _bankingRepositoryMock.Setup(x => x.GetAccounts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(bankAccounts);

        var query = new GetAllBankAccountsQuery { UserId = "ABC123", Limit = 10, Offset = 0 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(bankAccounts, result);
        _bankingRepositoryMock.Verify(x => x.GetAccounts(1, query.Limit, query.Offset), Times.Once);
    }
}