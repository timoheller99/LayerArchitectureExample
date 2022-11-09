namespace LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Persistence.Contracts;

using System;
using System.Threading.Tasks;

using LayerArchitectureExample.DataAccess.Contracts.Todo;

public interface ITodoDataAccessPersistenceHelper
{
    public Task<TodoDbModel> PersistTodoAsync(Guid? todoListId = null);

    public Task<TodoDbModel> PersistTodoAsync(TodoDbModel todoDbModel);
}
