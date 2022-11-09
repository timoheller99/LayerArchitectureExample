namespace LayerArchitectureExample.DataAccess.IntegrationTests.Todo.Tests;

using System;
using System.Threading.Tasks;

using FluentAssertions;

using LayerArchitectureExample.DataAccess.Core.Exceptions;

using Xunit;

public abstract partial class DataAccessTests<TFixture, TRepository, TDbModel>
{
    [Fact]
    public async Task GetByTodoListIdAsync_ExistingEntity_ShouldGetExistingEntity()
    {
        // Arrange
        var repository = this.Fixture.DataAccessServiceHelper.CreateTodoRepository();

        var existingDbModel = await this.Fixture.DataAccessPersistenceHelper.PersistTodoAsync();

        // Act
        var fetchedEntities = await repository.GetByTodoListIdAsync(existingDbModel.TodoListId);

        // Assert
        fetchedEntities.Should().ContainEquivalentOf(existingDbModel);
    }

    [Fact]
    public async Task GetByTodoListIdAsync_InvalidParameter_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.DataAccessServiceHelper.CreateTodoRepository();

        // Act
        var sut = async () => await repository.GetByTodoListIdAsync(Guid.Empty);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();
    }

    [Fact]
    public async Task GetByJobPositionIdAsync_NotExistingTodoListId_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.DataAccessServiceHelper.CreateTodoRepository();

        // Act
        var fetchedEntities = await repository.GetByTodoListIdAsync(Guid.NewGuid());

        // Assert
        fetchedEntities.Should().BeEmpty();
    }
}
