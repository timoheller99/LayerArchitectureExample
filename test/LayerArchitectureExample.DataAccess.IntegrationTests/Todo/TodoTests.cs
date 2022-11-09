namespace LayerArchitectureExample.DataAccess.IntegrationTests.Todo;

using LayerArchitectureExample.DataAccess.Contracts.Todo;
using LayerArchitectureExample.DataAccess.IntegrationTests.Todo.Tests;

using Xunit;

[Trait("Category", "DataAccess.IntegrationTests.Todo")]
[Collection("DataAccess.IntegrationTests.Todo")]
public class TodoTests : DataAccessTests<TodoTestFixture, ITodoRepository, TodoDbModel>
{
    public TodoTests(TodoTestFixture fixture)
        : base(fixture)
    {
    }
}
