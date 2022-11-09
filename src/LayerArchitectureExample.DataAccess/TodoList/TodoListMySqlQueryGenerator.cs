namespace LayerArchitectureExample.DataAccess.TodoList;

using System;

using LayerArchitectureExample.DataAccess.Contracts.TodoList;
using LayerArchitectureExample.DataAccess.Core;

public class TodoListMySqlQueryGenerator : MySqlQueryGenerator<Guid, TodoListDbModel>, ITodoListSqlQueryGenerator
{
    public TodoListMySqlQueryGenerator()
        : base("TodoList")
    {
    }

    public string GenerateSelectByNameQuery()
    {
        return this.GenerateSelectByPropertyQuery(nameof(TodoListDbModel.Name));
    }
}
