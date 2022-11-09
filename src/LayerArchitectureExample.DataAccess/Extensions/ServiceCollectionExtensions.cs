namespace LayerArchitectureExample.DataAccess.Extensions;

using LayerArchitectureExample.DataAccess.Contracts.Core;
using LayerArchitectureExample.DataAccess.Contracts.Todo;
using LayerArchitectureExample.DataAccess.Contracts.TodoList;
using LayerArchitectureExample.DataAccess.Core;
using LayerArchitectureExample.DataAccess.Core.Mapping;
using LayerArchitectureExample.DataAccess.Todo;
using LayerArchitectureExample.DataAccess.TodoList;
using LayerArchitectureExample.DataAccess.Validation.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDataAccess(this IServiceCollection services)
    {
        services.TryAddScoped<IDbConnectionFactory, MySqlConnectionFactory>();

        SetupCustomSqlTypeHandlers();

        services.AddValidation();

        services.AddTodoList();
        services.AddTodo();
    }

    private static void SetupCustomSqlTypeHandlers()
    {
        SqlGuidTypeHandler.Setup();
        SqlDateOnlyTypeHandler.Setup();
        SqlNullableDateOnlyTypeHandler.Setup();
    }

    private static void AddTodoList(this IServiceCollection services)
    {
        services.AddScoped<ITodoListSqlQueryGenerator, TodoListMySqlQueryGenerator>();
        services.AddScoped<ITodoListRepository, TodoListRepository>();
    }

    private static void AddTodo(this IServiceCollection services)
    {
        services.AddScoped<ITodoSqlQueryGenerator, TodoMySqlQueryGenerator>();
        services.AddScoped<ITodoRepository, TodoRepository>();
    }
}
