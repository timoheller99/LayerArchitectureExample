namespace LayerArchitectureExample.DataAccess.IntegrationTests.Todo.Tests;

using System;
using System.Threading.Tasks;

using FluentAssertions;

using LayerArchitectureExample.DataAccess.Core.Exceptions;

using Xunit;

public abstract partial class DataAccessTests<TFixture, TRepository, TDbModel>
{
    [Fact]
    public async Task GetByNameAsync_ExistingEntity_ShouldGetExistingEntity()
    {
        // Arrange
        var repository = this.Fixture.DataAccessServiceHelper.CreateTodoRepository();

        var existingDbModel = await this.Fixture.DataAccessPersistenceHelper.PersistTodoAsync();

        // Act
        var fetchedEntity = await repository.GetByNameAsync(existingDbModel.Name);

        // Assert
        fetchedEntity.Should().BeEquivalentTo(existingDbModel);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetByNameAsync_InvalidParameter_ShouldThrow(string parameter)
    {
        // Arrange
        var repository = this.Fixture.DataAccessServiceHelper.CreateTodoRepository();

        // Act
        var sut = async () => await repository.GetByNameAsync(parameter);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();
    }

    [Fact]
    public async Task GetByNameAsync_NotExistingEntity_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.DataAccessServiceHelper.CreateTodoRepository();

        // Act
        var sut = async () => await repository.GetByNameAsync(Guid.NewGuid().ToString());

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();
    }
}
