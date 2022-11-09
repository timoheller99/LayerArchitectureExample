namespace LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Service;

using System;

using LayerArchitectureExample.Core.Logging.Extensions;
using LayerArchitectureExample.Core.Validation.Contracts;
using LayerArchitectureExample.DataAccess.Contracts.Core;
using LayerArchitectureExample.DataAccess.Contracts.Todo;
using LayerArchitectureExample.DataAccess.Contracts.TodoList;
using LayerArchitectureExample.DataAccess.Core;
using LayerArchitectureExample.DataAccess.Extensions;
using LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Service.Contracts;
using LayerArchitectureExample.DataAccess.Todo;
using LayerArchitectureExample.DataAccess.TodoList;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;

public class DataAccessServiceHelper : IDataAccessServiceHelper
{
    private readonly IServiceProvider serviceProvider;

    private readonly IDbConnectionFactory dbConnectionFactory;

    public DataAccessServiceHelper(string dbConnectionString)
    {
        var services = new ServiceCollection();

        var configuration = this.CreateConfiguration();
        var hostEnvironment = this.CreateHostEnvironment();

        services.AddSerilogLogging(configuration, hostEnvironment);
        services.AddDataAccess();

        this.serviceProvider = services.BuildServiceProvider();
        this.dbConnectionFactory = new MySqlConnectionFactory(dbConnectionString);
    }

    public ITodoListRepository CreateTodoListRepository()
    {
        return new TodoListRepository(
            this.dbConnectionFactory,
            this.serviceProvider.GetRequiredService<ITodoListSqlQueryGenerator>(),
            this.serviceProvider.GetRequiredService<IValidationService<TodoListDbModel>>(),
            this.serviceProvider.GetRequiredService<ILogger<TodoListRepository>>());
    }

    public ITodoRepository CreateTodoRepository()
    {
        return new TodoRepository(
            this.dbConnectionFactory,
            this.serviceProvider.GetRequiredService<ITodoSqlQueryGenerator>(),
            this.serviceProvider.GetRequiredService<IValidationService<TodoDbModel>>(),
            this.serviceProvider.GetRequiredService<ILogger<TodoRepository>>());
    }

    private IHostEnvironment CreateHostEnvironment()
    {
        var environment = new HostingEnvironment
        {
            EnvironmentName = "Development",
        };

        return environment;
    }

    private IConfiguration CreateConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false);

        return configurationBuilder.Build();
    }
}
