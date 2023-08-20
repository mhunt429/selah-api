using FluentAssertions;
using NSubstitute;
using Selah.Application.Queries;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.Transactions.Sql;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;

namespace Selah.Application.UnitTests.Transactions;

public class TransactionTotalsByCategoryQueryUnitTests
{
    private readonly ITransactionRepository _transactionRepositoryMock;
    private readonly ISecurityService _securityServiceMock;

    public TransactionTotalsByCategoryQueryUnitTests()
    {
        _transactionRepositoryMock = Substitute.For<ITransactionRepository>();
        _securityServiceMock = Substitute.For<ISecurityService>();

        _transactionRepositoryMock.GetTransactionTotalsByCategory(Arg.Any<int>()).Returns(
            new List<TransactionAmountByCategorySql>()
            {
                new()
                {
                    Id = 1,
                    Name = "Category 1",
                    Total = 100
                },
                new()
                {
                    Id = 2,
                    Name = "Category 2",
                    Total = 100
                }
            });

        _securityServiceMock.EncodeHashId(Arg.Any<int>()).Returns("hashed id");
    }

    [Fact]
    public async Task Handler_Returns_TransactionAmountByCategoryList()
    {
        //Arrange
        var handler =
            new TransactionTotalsByCategoryQuery.Handler(_transactionRepositoryMock,
                _securityServiceMock);
        var query = new TransactionTotalsByCategoryQuery { UserId = "1234" };

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.Should().NotBeEmpty();
        result.Count().Should().Be(2);

        foreach (var item in result)
        {
            item.Id.Should().NotBeEmpty();
            item.Name.Should().NotBeEmpty();
            item.Total.Should().Be(100);
        }
    }
}