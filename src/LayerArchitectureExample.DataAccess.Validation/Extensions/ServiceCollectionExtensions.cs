namespace LayerArchitectureExample.DataAccess.Validation.Extensions;

using LayerArchitectureExample.Core.Validation;
using LayerArchitectureExample.Core.Validation.Contracts;
using LayerArchitectureExample.DataAccess.Contracts.Todo;
using LayerArchitectureExample.DataAccess.Contracts.TodoList;
using LayerArchitectureExample.DataAccess.Validation.Todo;
using LayerArchitectureExample.DataAccess.Validation.TodoList;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddValidation(this IServiceCollection services)
    {
        services.AddTodoList();
        services.AddTodo();
    }

    private static void AddTodoList(this IServiceCollection services)
    {
        services.AddScoped<TodoListDbModelValidator>();
        services.AddScoped<IValidationService<TodoListDbModel>, ValidationService<TodoListDbModelValidator, TodoListDbModel>>();
    }

    private static void AddTodo(this IServiceCollection services)
    {
        services.AddScoped<TodoDbModelValidator>();
        services.AddScoped<IValidationService<TodoDbModel>, ValidationService<TodoDbModelValidator, TodoDbModel>>();
    }
}
