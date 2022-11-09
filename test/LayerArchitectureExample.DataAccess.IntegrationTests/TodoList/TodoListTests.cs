namespace LayerArchitectureExample.DataAccess.IntegrationTests.TodoList;

using LayerArchitectureExample.DataAccess.Contracts.TodoList;
using LayerArchitectureExample.DataAccess.IntegrationTests.TodoList.Tests;

using Xunit;

[Trait("Category", "DataAccess.IntegrationTests.TodoList")]
[Collection("DataAccess.IntegrationTests.TodoList")]
public class TodoListTests : DataAccessTests<TodoListTestFixture, ITodoListRepository, TodoListDbModel>
{
    public TodoListTests(TodoListTestFixture fixture)
        : base(fixture)
    {
    }
}
