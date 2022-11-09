namespace LayerArchitectureExample.DataAccess.IntegrationTests.Todo;

using System;
using System.Threading.Tasks;

using LayerArchitectureExample.DataAccess.Contracts.Todo;
using LayerArchitectureExample.DataAccess.IntegrationTests.Core;
using LayerArchitectureExample.DataAccess.TestHelpers.Database;

using Xunit;

public class TodoTestFixture : BaseTestFixture<ITodoRepository, TodoDbModel>, IAsyncLifetime
{
    public TodoTestFixture()
        : base(new DatabaseHelper())
    {
    }

    public async Task InitializeAsync()
    {
        await this.Setup();
    }

    public override ITodoRepository GetRepository()
    {
        return this.DataAccessServiceHelper.CreateTodoRepository();
    }

    public override TodoDbModel GetValidDbModel()
    {
        var existingTodoList = this.DataAccessPersistenceHelper.PersistTodoListAsync().GetAwaiter().GetResult();
        return this.DataAccessModelHelper.CreateValidTodoDbModel(existingTodoList.Id);
    }

    public override TodoDbModel GetInvalidDbModel()
    {
        return this.DataAccessModelHelper.CreateInvalidTodoDbModel();
    }

    public override async Task<TodoDbModel> PersistDbModel()
    {
        return await this.DataAccessPersistenceHelper.PersistTodoAsync();
    }

    public override void PerformValidUpdate(ref TodoDbModel entity)
    {
        entity.Name = Guid.NewGuid().ToString();
    }

    public override void PerformInvalidUpdate(ref TodoDbModel entity)
    {
        entity.Name = null;
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await this.DisposeAsync();
    }
}
