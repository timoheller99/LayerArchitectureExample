namespace LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Persistence.Contracts;

using System.Threading.Tasks;

using LayerArchitectureExample.DataAccess.Contracts.TodoList;

public interface ITodoListDataAccessPersistenceHelper
{
    public Task<TodoListDbModel> PersistTodoListAsync();

    public Task<TodoListDbModel> PersistTodoListAsync(TodoListDbModel todoListDbModel);
}
