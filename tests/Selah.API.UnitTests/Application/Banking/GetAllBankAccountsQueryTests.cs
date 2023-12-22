using FluentAssertions;
using NSubstitute;
using Selah.Application.Queries.Banking;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.Banking;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;

namespace Selah.Application.UnitTests.Banking;

public class GetAllBankAccountsQueryTests
{
    private readonly IBankingRepository _bankingRepositoryMock;
    private readonly GetAllBankAccountsQuery.Handler _handler;

    public GetAllBankAccountsQueryTests()
    {
        _bankingRepositoryMock = Substitute.For<IBankingRepository>();
        ISecurityService securityServiceMock = Substitute.For<ISecurityService>();
        
        securityServiceMock.DecodeHashId(Arg.Any<string>()).Returns(1);
        securityServiceMock.EncodeHashId(Arg.Any<int>()).Returns("ABC123");
        _handler = new GetAllBankAccountsQuery.Handler(_bankingRepositoryMock, securityServiceMock);
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
                AccountName = "Checking",
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
                AccountName = "Savings",
                AvailableBalance = 100,
                CurrentBalance = 50,
                Subtype = "Savings",
                InstitutionId = 1,
                AccountMask = "****1234"
            }
        };
        _bankingRepositoryMock.GetAccounts(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>())
            .Returns(bankAccounts);

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
    }
}