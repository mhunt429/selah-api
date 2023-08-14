using FluentAssertions;
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
        var bankAccounts = new List<BankAccountSql>
        {
            new()
            {
                Id = 1,
                UserId = 1,
                Name = "Checking",
                AvailableBalance = 100,
                CurrentBalance = 50,
                Subtype = "Checking",
                InstitutionId = 1,
                AccountMask = "****1234"
            },
            new()
            {
                Id = 2,
                UserId = 1,
                Name = "Savings",
                AvailableBalance = 100,
                CurrentBalance = 50,
                Subtype = "Savings",
                InstitutionId = 1,
                AccountMask = "****1234"
            }
        };
        _bankingRepositoryMock.Setup(x => x.GetAccounts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(bankAccounts);

        var query = new GetAllBankAccountsQuery { UserId = "ABC123", Limit = 10, Offset = 0 };
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        foreach (var account in result)
        {
            account.Id.Should().NotBeEmpty();
            account.UserId.Should().NotBeEmpty();
            account.Name.Should().NotBeEmpty();
            account.Subtype.Should().NotBeEmpty();
            account.AccountMask.Should().Be("****1234");
            account.AvailableBalance.Should().Be(100);
            account.CurrentBalance.Should().Be(50);
        }

        _bankingRepositoryMock.Verify(x => x.GetAccounts(1, query.Limit, query.Offset), Times.Once);
    }
}