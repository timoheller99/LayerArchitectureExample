namespace LayerArchitectureExample.DataAccess.IntegrationTests.Core.Tests;

using System.Threading.Tasks;

using FluentAssertions;

using LayerArchitectureExample.DataAccess.Core.Exceptions;

using MySqlConnector;

using Xunit;

public abstract partial class BaseTests<TFixture, TRepository, TDbModel>
{
    [Fact]
    public async Task CreateAsync_ValidModel_ShouldPersistEntity()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var validDbModel = this.Fixture.GetValidDbModel();

        // Act
        await repository.CreateAsync(validDbModel);

        // Assert
        var createdEntity = await repository.GetByIdAsync(validDbModel.Id);

        createdEntity.Should().BeEquivalentTo(validDbModel);
    }

    [Fact]
    public async Task CreateAsync_InvalidModel_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var invalidDbModel = this.Fixture.GetInvalidDbModel();

        // Act
        var sut = async () => await repository.CreateAsync(invalidDbModel);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();

        var sut2 = async () => await repository.GetByIdAsync(invalidDbModel.Id);

        await sut2.Should().ThrowExactlyAsync<DataAccessException>();
    }

    [Fact]
    public async Task CreateAsync_Null_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var invalidDbModel = this.Fixture.GetInvalidDbModel();

        // Act
        var sut = async () => await repository.CreateAsync(null);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();

        var sut2 = async () => await repository.GetByIdAsync(invalidDbModel.Id);

        await sut2.Should().ThrowExactlyAsync<DataAccessException>();
    }

    [Fact]
    public async Task CreateAsync_NullConnection_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var validDbModel = this.Fixture.GetValidDbModel();

        // Act
        var sut = async () => await repository.CreateAsync(validDbModel, null);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();

        var sut2 = async () => await repository.GetByIdAsync(validDbModel.Id);

        await sut2.Should().ThrowExactlyAsync<DataAccessException>();
    }

    [Fact]
    public async Task CreateAsync_NullTransaction_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var validDbModel = this.Fixture.GetValidDbModel();

        // Act
        var sut = async () => await repository.CreateAsync(validDbModel, new MySqlConnection(), null);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();

        var sut2 = async () => await repository.GetByIdAsync(validDbModel.Id);

        await sut2.Should().ThrowExactlyAsync<DataAccessException>();
    }
}
