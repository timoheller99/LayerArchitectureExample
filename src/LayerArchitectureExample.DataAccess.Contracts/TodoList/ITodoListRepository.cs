﻿namespace LayerArchitectureExample.DataAccess.Contracts.TodoList;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using LayerArchitectureExample.DataAccess.Contracts.Core;

public interface ITodoListRepository : IGenericRepository<Guid, TodoListDbModel>
{
    public Task<TodoListDbModel> GetByNameAsync(string name);

    public Task<TodoListDbModel> GetByNameAsync(string name, IDbConnection connection);

    public Task<TodoListDbModel> GetByNameAsync(string name, IDbConnection connection, IDbTransaction transaction);
}
