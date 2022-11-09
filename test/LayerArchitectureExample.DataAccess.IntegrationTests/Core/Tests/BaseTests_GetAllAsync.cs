namespace LayerArchitectureExample.DataAccess.IntegrationTests.Core.Tests;

using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using LayerArchitectureExample.DataAccess.Core.Exceptions;

using MySqlConnector;

using Xunit;

public abstract partial class BaseTests<TFixture, TRepository, TDbModel>
{
    [Fact]
    public async Task GetAllAsync_OneExistingEntity_ShouldFetchExistingEntity()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var existingDbModel = await this.Fixture.PersistDbModel();

        // Act
        var entities = await repository.GetAllAsync();

        // Assert
        var entityList = entities.ToList();

        entityList.Should().ContainEquivalentOf(existingDbModel);
    }

    [Fact]
    public async Task GetAllAsync_TwoExistingEntity_ShouldFetchBothExistingEntities()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        var existingDbModel = await this.Fixture.PersistDbModel();
        var existingDbModel2 = await this.Fixture.PersistDbModel();

        // Act
        var entities = await repository.GetAllAsync();

        // Assert
        var entityList = entities.ToList();

        entityList.Should().ContainEquivalentOf(existingDbModel);
        entityList.Should().ContainEquivalentOf(existingDbModel2);
    }

    [Fact]
    public async Task GetAllAsync_NullConnection_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        // Act
        var sut = async () => await repository.GetAllAsync(null);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();
    }

    [Fact]
    public async Task GetAllAsync_NullTransaction_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.GetRepository();

        // Act
        var sut = async () => await repository.GetAllAsync(new MySqlConnection(), null);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();
    }
}
