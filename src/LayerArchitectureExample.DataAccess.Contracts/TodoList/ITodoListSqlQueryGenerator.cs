namespace LayerArchitectureExample.DataAccess.Contracts.TodoList;

using LayerArchitectureExample.DataAccess.Contracts.Core;

public interface ITodoListSqlQueryGenerator : ISqlQueryGenerator
{
    public string GenerateSelectByNameQuery();
}
