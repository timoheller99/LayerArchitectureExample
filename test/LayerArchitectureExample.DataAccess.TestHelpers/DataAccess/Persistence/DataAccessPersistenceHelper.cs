namespace LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Persistence;

using System;
using System.Threading.Tasks;

using LayerArchitectureExample.DataAccess.Contracts.Todo;
using LayerArchitectureExample.DataAccess.Contracts.TodoList;
using LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Model.Contracts;
using LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Persistence.Contracts;
using LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Service.Contracts;

public class DataAccessPersistenceHelper : IDataAccessPersistenceHelper
{
    private readonly IDataAccessModelHelper modelHelper;

    private readonly IDataAccessServiceHelper serviceHelper;

    public DataAccessPersistenceHelper(IDataAccessModelHelper modelHelper, IDataAccessServiceHelper serviceHelper)
    {
        this.modelHelper = modelHelper;
        this.serviceHelper = serviceHelper;
    }

    public async Task<TodoListDbModel> PersistTodoListAsync()
    {
        var model = this.modelHelper.CreateValidTodoListDbModel();

        return await this.PersistTodoListAsync(model);
    }

    public async Task<TodoListDbModel> PersistTodoListAsync(TodoListDbModel todoListDbModel)
    {
        ArgumentNullException.ThrowIfNull(todoListDbModel);

        var repository = this.serviceHelper.CreateTodoListRepository();

        await repository.CreateAsync(todoListDbModel);

        return await repository.GetByIdAsync(todoListDbModel.Id);
    }

    public async Task<TodoDbModel> PersistTodoAsync(Guid? todoListId = null)
    {
        var model = this.modelHelper.CreateValidTodoDbModel(
            todoListId ?? (await this.PersistTodoListAsync()).Id);

        return await this.PersistTodoAsync(model);
    }

    public async Task<TodoDbModel> PersistTodoAsync(TodoDbModel todoDbModel)
    {
        ArgumentNullException.ThrowIfNull(todoDbModel);

        var repository = this.serviceHelper.CreateTodoRepository();

        await repository.CreateAsync(todoDbModel);

        return await repository.GetByIdAsync(todoDbModel.Id);
    }
}
