namespace LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Model.Contracts;

using LayerArchitectureExample.DataAccess.Contracts.TodoList;

public interface ITodoListDataAccessModelHelper
{
    public TodoListDbModel CreateValidTodoListDbModel();

    public TodoListDbModel CreateInvalidTodoListDbModel();
}
