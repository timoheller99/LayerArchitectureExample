﻿namespace LayerArchitectureExample.DataAccess.IntegrationTests.Todo.Tests;

using System.Threading.Tasks;

using FluentAssertions;

using LayerArchitectureExample.DataAccess.Core.Exceptions;

using Xunit;

public abstract partial class DataAccessTests<TFixture, TRepository, TDbModel>
{
    [Fact]
    public async Task CreateAsync_DuplicateName_ShouldThrow()
    {
        // Arrange
        var repository = this.Fixture.DataAccessServiceHelper.CreateTodoRepository();

        var existingDbModel = await this.Fixture.DataAccessPersistenceHelper.PersistTodoAsync();

        var existingTodoList = await this.Fixture.DataAccessPersistenceHelper.PersistTodoListAsync();

        var validDbModel = this.Fixture.DataAccessModelHelper.CreateValidTodoDbModel(existingTodoList.Id);
        validDbModel.Name = existingDbModel.Name;

        // Act
        var sut = async () => await repository.CreateAsync(validDbModel);

        // Assert
        await sut.Should().ThrowExactlyAsync<DataAccessException>();

        var sut2 = async () => await repository.GetByIdAsync(validDbModel.Id);
        await sut2.Should().ThrowExactlyAsync<DataAccessException>();
    }
}
