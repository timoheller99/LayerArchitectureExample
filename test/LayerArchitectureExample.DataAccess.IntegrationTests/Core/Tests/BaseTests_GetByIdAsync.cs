namespace LayerArchitectureExample.DataAccess.IntegrationTests.Core.Tests;

using System;
using System.Threading.Tasks;

using FluentAssertions;

using LayerArchitectureExample.DataAccess.Core.Exceptions;

using MySqlConnector;

using Xunit;

public abstract partial class BaseTests<TFixture, TRepository, TDbModel>
{
    [Fact]
    public async Task GetByIdAsync_ExistingEntity_ShouldGetExistingEntity()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var existingDbModel = await this.Fixture.PersistDbModel();

        // Act
        var fetchedEntity = await repository.GetByIdAsync(existingDbModel.Id);

        // Assert
        fetchedEntity.Should().BeEquivalentTo(existingDbModel);
    }

    [Fact]
    public async Task GetByIdAsync_NotExistingEntity_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var notExistingDbModel = this.Fixture.GetValidDbModel();

        // Act
        var sut = async () => await repository.GetByIdAsync(notExistingDbModel.Id);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        // Act
        var sut = async () => await repository.GetByIdAsync(Guid.Empty);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();
    }

    [Fact]
    public async Task GetByIdAsync_NullConnection_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var existingDbModel = await this.Fixture.PersistDbModel();

        // Act
        var sut = async () => await repository.GetByIdAsync(existingDbModel.Id, null);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();
    }

    [Fact]
    public async Task GetByIdAsync_NullTransaction_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var existingDbModel = await this.Fixture.PersistDbModel();

        // Act
        var sut = async () => await repository.GetByIdAsync(existingDbModel.Id, new MySqlConnection(), null);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();
    }
}
