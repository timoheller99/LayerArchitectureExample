namespace LayerArchitectureExample.DataAccess.Core;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using Dapper;

using FluentValidation;

using LayerArchitectureExample.Core.Logging.Extensions;
using LayerArchitectureExample.Core.Validation.Contracts;
using LayerArchitectureExample.DataAccess.Contracts.Core;
using LayerArchitectureExample.DataAccess.Core.Exceptions;
using LayerArchitectureExample.DataAccess.Core.Helpers;
using LayerArchitectureExample.DataAccess.Validation.Core;

using Microsoft.Extensions.Logging;

public class GenericRepository<TRepository, TDbModel> : IGenericRepository<Guid, TDbModel>
    where TDbModel : IDbModel<Guid>
{
    private readonly string deleteQuery;

    private readonly string insertQuery;

    private readonly string selectAllQuery;

    private readonly string selectByIdQuery;

    private readonly string updateQuery;

    protected GenericRepository(
        IDbConnectionFactory dbConnectionFactory,
        ISqlQueryGenerator sqlQueryGenerator,
        IValidationService<TDbModel> validationService,
        ILogger<TRepository> logger,
        string entityName)
    {
        this.DbConnectionFactory = dbConnectionFactory;
        this.ValidationService = validationService;
        this.Logger = logger;

        this.EntityName = entityName;

        this.insertQuery = sqlQueryGenerator.GenerateInsertQuery();
        this.selectAllQuery = sqlQueryGenerator.GenerateSelectAllQuery();
        this.selectByIdQuery = sqlQueryGenerator.GenerateSelectByIdQuery();
        this.updateQuery = sqlQueryGenerator.GenerateUpdateQuery();
        this.deleteQuery = sqlQueryGenerator.GenerateDeleteQuery();
    }

    protected string EntityName { get; }

    protected IDbConnectionFactory DbConnectionFactory { get; }

    protected IValidationService<TDbModel> ValidationService { get; }

    protected ILogger<TRepository> Logger { get; }

    public async Task CreateAsync(TDbModel entity)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(entity);

            using var connection = this.DbConnectionFactory.CreateDbConnection();
            connection.Open();

            await this.CreateAsync(entity, connection);
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName}: {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task CreateAsync(TDbModel entity, IDbConnection connection)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(connection);

            using var transaction = connection.BeginTransaction();

            try
            {
                await this.CreateAsync(entity, connection, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName}: {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task CreateAsync(TDbModel entity, IDbConnection connection, IDbTransaction transaction)
    {
        using var log = this.Logger.TimedOperation(LogLevel.Information, "{ClassName}.{MethodName} {PropertyName}: {@PropertyValue}", typeof(TRepository).Name, MethodNameHelper.GetCallerName(), typeof(TDbModel).Name, entity);

        try
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(connection);
            ArgumentNullException.ThrowIfNull(transaction);

            var validationResult = await this.ValidationService.ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                throw new ValidationException($"Invalid {this.EntityName} model", validationResult.Errors);
            }

            await connection.ExecuteAsync(this.insertQuery, entity, transaction: transaction);
        }
        catch (Exception e)
        {
            log.Exception = e;
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName}: {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task<IEnumerable<TDbModel>> GetAllAsync()
    {
        try
        {
            using var connection = this.DbConnectionFactory.CreateDbConnection();
            connection.Open();

            return await this.GetAllAsync(connection);
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName}: {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task<IEnumerable<TDbModel>> GetAllAsync(IDbConnection connection)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(connection);

            using var transaction = connection.BeginTransaction();

            return await this.GetAllAsync(connection, transaction);
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName}: {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task<IEnumerable<TDbModel>> GetAllAsync(IDbConnection connection, IDbTransaction transaction)
    {
        using var log = this.Logger.TimedOperation(LogLevel.Information, "{ClassName}.{MethodName}", typeof(TRepository).Name, MethodNameHelper.GetCallerName());

        try
        {
            ArgumentNullException.ThrowIfNull(connection);
            ArgumentNullException.ThrowIfNull(transaction);

            var entities = await connection.QueryAsync<TDbModel>(this.selectAllQuery, transaction: transaction);

            return entities;
        }
        catch (Exception e)
        {
            log.Exception = e;
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName}: {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task<TDbModel> GetByIdAsync(Guid id)
    {
        try
        {
            using var connection = this.DbConnectionFactory.CreateDbConnection();
            connection.Open();

            return await this.GetByIdAsync(id, connection);
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(IDbModel<Guid>.Id)}'='{id}': {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task<TDbModel> GetByIdAsync(Guid id, IDbConnection connection)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(connection);

            using var transaction = connection.BeginTransaction();

            try
            {
                return await this.GetByIdAsync(id, connection, transaction);
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(IDbModel<Guid>.Id)}'='{id}': {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task<TDbModel> GetByIdAsync(Guid id, IDbConnection connection, IDbTransaction transaction)
    {
        using var log = this.Logger.TimedOperation(LogLevel.Information, "{ClassName}.{MethodName} {PropertyName}: {PropertyValue}", typeof(TRepository).Name, MethodNameHelper.GetCallerName(), nameof(IDbModel<Guid>.Id), id);

        try
        {
            ArgumentNullException.ThrowIfNull(connection);
            ArgumentNullException.ThrowIfNull(transaction);

            var validationResult = await new GuidValidator().ValidateAsync(id);
            if (!validationResult.IsValid)
            {
                throw new ValidationException($"Invalid {nameof(IDbModel<Guid>.Id)}", validationResult.Errors);
            }

            var entity = await connection.QuerySingleOrDefaultAsync<TDbModel>(this.selectByIdQuery, new { id, }, transaction: transaction);
            if (entity == null)
            {
                throw new EntityNotFoundException($"Could not find '{this.EntityName}' with '{nameof(IDbModel<Guid>.Id)}'='{id}'");
            }

            return entity;
        }
        catch (Exception e)
        {
            log.Exception = e;
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(IDbModel<Guid>.Id)}'='{id}': {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task UpdateAsync(TDbModel entity)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(entity);

            using var connection = this.DbConnectionFactory.CreateDbConnection();
            connection.Open();

            await this.UpdateAsync(entity, connection);
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(IDbModel<Guid>.Id)}'='{entity?.Id}': {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task UpdateAsync(TDbModel entity, IDbConnection connection)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(connection);

            using var transaction = connection.BeginTransaction();

            try
            {
                await this.UpdateAsync(entity, connection, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(IDbModel<Guid>.Id)}'='{entity.Id}': {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task UpdateAsync(TDbModel entity, IDbConnection connection, IDbTransaction transaction)
    {
        using var log = this.Logger.TimedOperation(LogLevel.Information, "{ClassName}.{MethodName} {PropertyName}: {@PropertyValue}", typeof(TRepository).Name, MethodNameHelper.GetCallerName(), typeof(TDbModel).Name, entity);

        try
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(connection);
            ArgumentNullException.ThrowIfNull(transaction);

            var validationResult = await this.ValidationService.ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                throw new ValidationException($"Invalid {this.EntityName} model", validationResult.Errors);
            }

            await this.GetByIdAsync(entity.Id);

            await connection.ExecuteAsync(this.updateQuery, entity, transaction: transaction);
        }
        catch (Exception e)
        {
            log.Exception = e;
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(IDbModel<Guid>.Id)}'='{entity.Id}': {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            using var connection = this.DbConnectionFactory.CreateDbConnection();
            connection.Open();

            await this.DeleteAsync(id, connection);
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(IDbModel<Guid>.Id)}'='{id}': {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task DeleteAsync(Guid id, IDbConnection connection)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(connection);

            using var transaction = connection.BeginTransaction();

            try
            {
                await this.DeleteAsync(id, connection, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(IDbModel<Guid>.Id)}'='{id}': {e.GetType()} - {e.Message}", e);
        }
    }

    public async Task DeleteAsync(Guid id, IDbConnection connection, IDbTransaction transaction)
    {
        using var log = this.Logger.TimedOperation(LogLevel.Information, "{ClassName}.{MethodName} {PropertyName}: {PropertyValue}", typeof(TRepository).Name, MethodNameHelper.GetCallerName(), nameof(IDbModel<Guid>.Id), id);

        try
        {
            ArgumentNullException.ThrowIfNull(connection);
            ArgumentNullException.ThrowIfNull(transaction);

            var validationResult = await new GuidValidator().ValidateAsync(id);
            if (!validationResult.IsValid)
            {
                throw new ValidationException($"Invalid {nameof(IDbModel<Guid>.Id)}", validationResult.Errors);
            }

            await this.GetByIdAsync(id);

            await connection.ExecuteAsync(this.deleteQuery, new { id, }, transaction: transaction);
        }
        catch (Exception e)
        {
            log.Exception = e;
            throw new DataAccessException($"Failed to execute {MethodNameHelper.GetCallerName()} for {this.EntityName} with '{nameof(IDbModel<Guid>.Id)}'='{id}': {e.GetType()} - {e.Message}", e);
        }
    }
}
