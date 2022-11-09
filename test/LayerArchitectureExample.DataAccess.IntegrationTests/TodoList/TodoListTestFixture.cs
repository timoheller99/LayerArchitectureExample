namespace LayerArchitectureExample.DataAccess.IntegrationTests.TodoList;

using System;
using System.Threading.Tasks;

using LayerArchitectureExample.DataAccess.Contracts.TodoList;
using LayerArchitectureExample.DataAccess.IntegrationTests.Core;
using LayerArchitectureExample.DataAccess.TestHelpers.Database;

using Xunit;

public class TodoListTestFixture : BaseTestFixture<ITodoListRepository, TodoListDbModel>, IAsyncLifetime
{
    public TodoListTestFixture()
        : base(new DatabaseHelper())
    {
    }

    public async Task InitializeAsync()
    {
        await this.Setup();
    }

    public override ITodoListRepository GetRepository()
    {
        return this.DataAccessServiceHelper.CreateTodoListRepository();
    }

    public override TodoListDbModel GetValidDbModel()
    {
        return this.DataAccessModelHelper.CreateValidTodoListDbModel();
    }

    public override TodoListDbModel GetInvalidDbModel()
    {
        return this.DataAccessModelHelper.CreateInvalidTodoListDbModel();
    }

    public override async Task<TodoListDbModel> PersistDbModel()
    {
        return await this.DataAccessPersistenceHelper.PersistTodoListAsync();
    }

    public override void PerformValidUpdate(ref TodoListDbModel entity)
    {
        entity.Name = Guid.NewGuid().ToString();
    }

    public override void PerformInvalidUpdate(ref TodoListDbModel entity)
    {
        entity.Name = null;
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await this.DisposeAsync();
    }
}
