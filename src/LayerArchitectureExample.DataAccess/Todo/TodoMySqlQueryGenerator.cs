namespace LayerArchitectureExample.DataAccess.Todo;

using System;

using LayerArchitectureExample.DataAccess.Contracts.Todo;
using LayerArchitectureExample.DataAccess.Core;

public class TodoMySqlQueryGenerator : MySqlQueryGenerator<Guid, TodoDbModel>, ITodoSqlQueryGenerator
{
    public TodoMySqlQueryGenerator()
        : base("Todo")
    {
    }

    public string GenerateSelectByNameQuery()
    {
        return this.GenerateSelectByPropertyQuery(nameof(TodoDbModel.Name));
    }

    public string GenerateSelectByTodoListIdQuery()
    {
        return this.GenerateSelectByPropertyQuery(nameof(TodoDbModel.TodoListId));
    }
}
