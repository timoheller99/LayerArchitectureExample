namespace LayerArchitectureExample.DataAccess.TodoList;

using System;
using System.Data;
using System.Threading.Tasks;

using Dapper;

using FluentValidation;

using LayerArchitectureExample.Core.Logging.Extensions;
using LayerArchitectureExample.Core.Validation.Contracts;
using LayerArchitectureExample.DataAccess.Contracts.Core;
using LayerArchitectureExample.DataAccess.Contracts.TodoList;
using LayerArchitectureExample.DataAccess.Core;
using LayerArchitectureExample.DataAccess.Core.Exceptions;
using LayerArchitectureExample.DataAccess.Core.Helpers;
using LayerArchitectureExample.DataAccess.Validation.Core;

using Microsoft.Extensions.Logging;

public class TodoListRepository : GenericRepository<TodoListRepository, TodoListDbModel>, ITodoListRepository
{
    private readonly string selectByNameQuery;

    public TodoListRepository(IDbConnectionFactory dbConnectionFactory, ITodoListSqlQueryGenerator sqlQueryGenerator, IValidationService<TodoListDbModel> validationService, ILogger<TodoListRepository> logger)
        : base(dbConnectionFactory, sqlQueryGenerator, validationService, logger, "TodoList")
    {
        this.selectByNameQuery = sqlQueryGenerator.GenerateSelectByNameQuery();
    }

    public async Task<TodoListDbModel> GetByNameAsync(string name)
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
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(TodoListDbModel.Name)}'='{name}': {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task<TodoListDbModel> GetByNameAsync(string name, IDbConnection connection)
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
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(TodoListDbModel.Name)}'='{name}': {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task<TodoListDbModel> GetByNameAsync(string name, IDbConnection connection, IDbTransaction transaction)
    {
        using var log = this.Logger.TimedOperation(LogLevel.Information, "{ClassName}.{MethodName} {PropertyName}: {PropertyValue}", nameof(this.GetType), MethodNameHelper.GetCallerName(), nameof(TodoListDbModel.Name), name);

        try
        {
            ArgumentNullException.ThrowIfNull(connection);
            ArgumentNullException.ThrowIfNull(transaction);

            var validationResult = await new NameValidator().ValidateAsync(name);
            if (!validationResult.IsValid)
            {
                throw new ValidationException($"Invalid {nameof(TodoListDbModel.Name)}", validationResult.Errors);
            }

            var entity = await connection.QuerySingleOrDefaultAsync<TodoListDbModel>(this.selectByNameQuery, new { name, }, transaction: transaction);
            if (entity == null)
            {
                throw new EntityNotFoundException($"Could not find '{this.EntityName}' with '{nameof(TodoListDbModel.Name)}'='{name}'");
            }

            return entity;
        }
        catch (Exception e)
        {
            log.Exception = e;
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(TodoListDbModel.Name)}'='{name}': {e.GetType()} - {e.Message}", e);
        }
    }
}
