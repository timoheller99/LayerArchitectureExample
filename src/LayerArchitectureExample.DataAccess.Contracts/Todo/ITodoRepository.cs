namespace LayerArchitectureExample.DataAccess.Contracts.Todo;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using LayerArchitectureExample.DataAccess.Contracts.Core;

public interface ITodoRepository : IGenericRepository<Guid, TodoDbModel>
{
    public Task<TodoDbModel> GetByNameAsync(string name);

    public Task<TodoDbModel> GetByNameAsync(string name, IDbConnection connection);

    public Task<TodoDbModel> GetByNameAsync(string name, IDbConnection connection, IDbTransaction transaction);

    public Task<IEnumerable<TodoDbModel>> GetByTodoListIdAsync(Guid todoListId);

    public Task<IEnumerable<TodoDbModel>> GetByTodoListIdAsync(Guid todoListId, IDbConnection connection);

    public Task<IEnumerable<TodoDbModel>> GetByTodoListIdAsync(Guid todoListId, IDbConnection connection, IDbTransaction transaction);
}
