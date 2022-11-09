namespace LayerArchitectureExample.DataAccess.Contracts.Todo;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using LayerArchitectureExample.DataAccess.Contracts.Core;

public interface ITodoRepository : IGenericRepository<Guid, TodoDbEntity>
{
    public Task<TodoDbEntity> GetByNameAsync(string name);

    public Task<TodoDbEntity> GetByNameAsync(string name, IDbConnection connection);

    public Task<TodoDbEntity> GetByNameAsync(string name, IDbConnection connection, IDbTransaction transaction);

    public Task<IEnumerable<TodoDbEntity>> GetByTodoListIdAsync(Guid todoListId);

    public Task<IEnumerable<TodoDbEntity>> GetByTodoListIdAsync(Guid todoListId, IDbConnection connection);

    public Task<IEnumerable<TodoDbEntity>> GetByTodoListIdAsync(Guid todoListId, IDbConnection connection, IDbTransaction transaction);
}
