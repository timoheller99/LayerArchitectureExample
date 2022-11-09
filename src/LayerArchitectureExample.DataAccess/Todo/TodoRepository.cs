namespace LayerArchitectureExample.DataAccess.Todo;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using Dapper;

using FluentValidation;

using LayerArchitectureExample.Core.Logging.Extensions;
using LayerArchitectureExample.Core.Validation.Contracts;
using LayerArchitectureExample.DataAccess.Contracts.Core;
using LayerArchitectureExample.DataAccess.Contracts.Todo;
using LayerArchitectureExample.DataAccess.Core;
using LayerArchitectureExample.DataAccess.Core.Exceptions;
using LayerArchitectureExample.DataAccess.Core.Helpers;
using LayerArchitectureExample.DataAccess.Validation.Core;

using Microsoft.Extensions.Logging;

public class TodoRepository : GenericRepository<TodoRepository, TodoDbModel>, ITodoRepository
{
    private readonly string selectByNameQuery;

    private readonly string selectByTodoListIdQuery;

    public TodoRepository(IDbConnectionFactory dbConnectionFactory, ITodoSqlQueryGenerator sqlQueryGenerator, IValidationService<TodoDbModel> validationService, ILogger<TodoRepository> logger)
        : base(dbConnectionFactory, sqlQueryGenerator, validationService, logger, "Todo")
    {
        this.selectByNameQuery = sqlQueryGenerator.GenerateSelectByNameQuery();
        this.selectByTodoListIdQuery = sqlQueryGenerator.GenerateSelectByTodoListIdQuery();
    }

    public async Task<TodoDbModel> GetByNameAsync(string name)
    {
        try
        {
            using var connection = this.DbConnectionFactory.CreateDbConnection();
            connection.Open();

            return await this.GetByNameAsync(name, connection);
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(TodoDbModel.Name)}'='{name}': {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task<TodoDbModel> GetByNameAsync(string name, IDbConnection connection)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(connection);

            using var transaction = connection.BeginTransaction();

            return await this.GetByNameAsync(name, connection, transaction);
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(TodoDbModel.Name)}'='{name}': {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task<TodoDbModel> GetByNameAsync(string name, IDbConnection connection, IDbTransaction transaction)
    {
        using var log = this.Logger.TimedOperation(LogLevel.Information, "{ClassName}.{MethodName} {PropertyName}: {PropertyValue}", nameof(this.GetType), MethodNameHelper.GetCallerName(), nameof(TodoDbModel.Name), name);

        try
        {
            ArgumentNullException.ThrowIfNull(connection);
            ArgumentNullException.ThrowIfNull(transaction);

            var validationResult = await new NameValidator().ValidateAsync(name);
            if (!validationResult.IsValid)
            {
                throw new ValidationException($"Invalid {nameof(TodoDbModel.Name)}", validationResult.Errors);
            }

            var entity = await connection.QuerySingleOrDefaultAsync<TodoDbModel>(this.selectByNameQuery, new { name, }, transaction: transaction);
            if (entity == null)
            {
                throw new EntityNotFoundException($"Could not find '{this.EntityName}' with '{nameof(TodoDbModel.Name)}'='{name}'");
            }

            return entity;
        }
        catch (Exception e)
        {
            log.Exception = e;
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(TodoDbModel.Name)}'='{name}': {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task<IEnumerable<TodoDbModel>> GetByTodoListIdAsync(Guid todoListId)
    {
        try
        {
            using var connection = this.DbConnectionFactory.CreateDbConnection();
            connection.Open();

            return await this.GetByTodoListIdAsync(todoListId, connection);
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(TodoDbModel.TodoListId)}'='{todoListId}': {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task<IEnumerable<TodoDbModel>> GetByTodoListIdAsync(Guid todoListId, IDbConnection connection)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(connection);

            using var transaction = connection.BeginTransaction();

            return await this.GetByTodoListIdAsync(todoListId, connection, transaction);
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(TodoDbModel.TodoListId)}'='{todoListId}': {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task<IEnumerable<TodoDbModel>> GetByTodoListIdAsync(Guid todoListId, IDbConnection connection, IDbTransaction transaction)
    {
        using var log = this.Logger.TimedOperation(LogLevel.Information, "{ClassName}.{MethodName} {PropertyName}: {PropertyValue}", nameof(this.GetType), MethodNameHelper.GetCallerName(), nameof(TodoDbModel.TodoListId), todoListId);

        try
        {
            ArgumentNullException.ThrowIfNull(connection);
            ArgumentNullException.ThrowIfNull(transaction);

            var validationResult = await new GuidValidator().ValidateAsync(todoListId);
            if (!validationResult.IsValid)
            {
                throw new ValidationException($"Invalid {nameof(TodoDbModel.TodoListId)}", validationResult.Errors);
            }

            var entities = await connection.QueryAsync<TodoDbModel>(this.selectByTodoListIdQuery, new { todoListId, }, transaction: transaction);
            if (entities == null)
            {
                throw new EntityNotFoundException($"Could not find '{this.EntityName}' with '{nameof(TodoDbModel.TodoListId)}'='{todoListId}'");
            }

            return entities;
        }
        catch (Exception e)
        {
            log.Exception = e;
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(TodoDbModel.TodoListId)}'='{todoListId}': {e.GetType()} - {e.Message}", e);
        }
    }
}
