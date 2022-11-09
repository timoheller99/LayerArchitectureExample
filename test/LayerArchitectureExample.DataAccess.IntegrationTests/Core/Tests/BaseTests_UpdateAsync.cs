namespace LayerArchitectureExample.DataAccess.IntegrationTests.Core.Tests;

using System.Threading.Tasks;

using FluentAssertions;

using LayerArchitectureExample.DataAccess.Core.Exceptions;

using MySqlConnector;

using Xunit;

public abstract partial class BaseTests<TFixture, TRepository, TDbModel>
{
    [Fact]
    public async Task UpdateAsync_ValidUpdate_ShouldGetExistingEntity()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var existingDbModel = await this.Fixture.PersistDbModel();

        // Act
        this.Fixture.PerformValidUpdate(ref existingDbModel);

        await repository.UpdateAsync(existingDbModel);

        // Assert
        var updatedEntity = await repository.GetByIdAsync(existingDbModel.Id);

        updatedEntity.Should().BeEquivalentTo(existingDbModel);
    }

    [Fact]
    public async Task UpdateAsync_InvalidUpdate_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var existingDbModel = await this.Fixture.PersistDbModel();

        // Act
        this.Fixture.PerformInvalidUpdate(ref existingDbModel);

        var sut = async () => await repository.UpdateAsync(existingDbModel);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();
    }

    [Fact]
    public async Task UpdateAsync_NotExistingEntity_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var notExistingEntity = this.Fixture.GetValidDbModel();

        // Act
        var sut = async () => await repository.UpdateAsync(notExistingEntity);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();
    }

    [Fact]
    public async Task UpdateAsync_Null_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        // Act
        var sut = async () => await repository.UpdateAsync(null);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();
    }

    [Fact]
    public async Task UpdateAsync_NullConnection_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var existingDbModel = await this.Fixture.PersistDbModel();

        // Act
        var sut = async () => await repository.UpdateAsync(existingDbModel, null);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();
    }

    [Fact]
    public async Task UpdateAsync_NullTransaction_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var existingDbModel = await this.Fixture.PersistDbModel();

        // Act
        var sut = async () => await repository.UpdateAsync(existingDbModel, new MySqlConnection(), null);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();
    }
}
