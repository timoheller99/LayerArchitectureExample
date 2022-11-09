namespace LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Service.Contracts;

using LayerArchitectureExample.DataAccess.Contracts.Todo;

public interface ITodoDataAccessServiceHelper
{
    public ITodoRepository CreateTodoRepository();
}
