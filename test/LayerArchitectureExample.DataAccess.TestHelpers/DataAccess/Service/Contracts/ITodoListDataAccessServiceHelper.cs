namespace LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Service.Contracts;

using LayerArchitectureExample.DataAccess.Contracts.TodoList;

public interface ITodoListDataAccessServiceHelper
{
    public ITodoListRepository CreateTodoListRepository();
}
