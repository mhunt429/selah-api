using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Selah.Infrastructure;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;

namespace Selah.Application.UnitTests.Infrastructure.RepositoryTests;

public class BaseRepositoryUnitTests
{
    private readonly IDbConnectionFactory _dbConnectionFactory =
        new NpgsqlConnectionFactory("My Test Connection String");

    private readonly ILogger<BaseRepository> _mockLogger = Substitute.For<ILogger<BaseRepository>>();
    
    [Fact]
    public void BaseRepository_CanBeInstantiated()
    {
        IBaseRepository baseRepository = new BaseRepository(_dbConnectionFactory, _mockLogger);
        baseRepository.Should().NotBeNull();
    }
}