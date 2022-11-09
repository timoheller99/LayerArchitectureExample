namespace LayerArchitectureExample.DataAccess.Contracts.Todo;

using LayerArchitectureExample.DataAccess.Contracts.Core;

public interface ITodoSqlQueryGenerator : ISqlQueryGenerator
{
    public string GenerateSelectByNameQuery();

    public string GenerateSelectByTodoListIdQuery();
}
