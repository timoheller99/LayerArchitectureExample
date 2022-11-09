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
    public async Task DeleteAsync_ExistingEntity_ShouldDeleteExistingEntity()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var existingTodo = await this.Fixture.PersistDbModel();

        // Act
        await repository.DeleteAsync(existingTodo.Id);

        // Assert
        var existingEntities = await repository.GetAllAsync();
        existingEntities.Should().NotContainEquivalentOf(existingTodo);
    }

    [Fact]
    public async Task DeleteAsync_NotExistingEntity_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var notExistingDbModel = this.Fixture.GetValidDbModel();

        // Act
        var sut = async () => await repository.DeleteAsync(notExistingDbModel.Id);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();
    }

    [Fact]
    public async Task DeleteAsync_InvalidValue_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        // Act
        var sut = async () => await repository.DeleteAsync(Guid.Empty);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();
    }

    [Fact]
    public async Task DeleteAsync_NullConnection_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var validDbModel = this.Fixture.GetValidDbModel();

        // Act
        var sut = async () => await repository.DeleteAsync(validDbModel.Id, null);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();

        var sut2 = async () => await repository.GetByIdAsync(validDbModel.Id);

        await sut2.Should().ThrowExactlyAsync<DataAccessException>();
    }

    [Fact]
    public async Task DeleteAsync_NullTransaction_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var validDbModel = this.Fixture.GetValidDbModel();

        // Act
        var sut = async () => await repository.DeleteAsync(validDbModel.Id, new MySqlConnection(), null);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();

        var sut2 = async () => await repository.GetByIdAsync(validDbModel.Id);

        await sut2.Should().ThrowExactlyAsync<DataAccessException>();
    }
}
