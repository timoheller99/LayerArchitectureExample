namespace LayerArchitectureExample.DataAccess.TestHelpers.Database;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

using LayerArchitectureExample.DataAccess.TestHelpers.Database.Contracts;

public class DatabaseContainerHelper : IDatabaseContainerHelper
{
    public DatabaseContainerHelper(IDatabaseConfig config)
    {
        this.Config = config;
    }

    protected IDatabaseConfig Config { get; init; }

    protected TestcontainersContainer Container { get; set; }

    public TestcontainersContainer CreateContainer()
    {
        {
            var containerBuilder = new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage("mysql:5.7")
                .WithName($"mysql_test_{Guid.NewGuid().ToString()}")
                .WithExposedPort(3306)
                .WithPortBinding(this.Config.ExposedPort, 3306)
                .WithEnvironment("MYSQL_ROOT_PASSWORD", this.Config.AdminPassword)
                .WithEnvironment("MYSQL_DATABASE", this.Config.DatabaseName)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(3306));

            this.Container = containerBuilder.Build();

            return this.Container;
        }
    }

    public async Task StartContainerAndSetupDatabaseAsync()
    {
        await this.Container.StartAsync();

        await this.SetupDatabaseInContainer();
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        if (this.Container != null)
        {
            await this.Container.StopAsync();
            await this.Container.DisposeAsync();
        }
    }

    protected async Task ExecuteSqlScriptInContainerAsync(string sqlFilePath)
    {
        ArgumentNullException.ThrowIfNull(this.Container);

        var initializeFileContent = await File.ReadAllTextAsync(sqlFilePath);
        await this.Container.ExecAsync(new List<string> { "mysql", "--user=root", $"--password={this.Config.AdminPassword}", "-e", initializeFileContent });
    }

    private async Task SetupDatabaseInContainer()
    {
        ArgumentNullException.ThrowIfNull(this.Container);

        var databaseFile = Path.Combine(this.Config.SqlScriptsPath, "database.sql");
        await this.ExecuteSqlScriptInContainerAsync(databaseFile);

        try
        {
            var initializeFile = Path.Combine(this.Config.SqlScriptsPath, "initialize.sql");
            await this.ExecuteSqlScriptInContainerAsync(initializeFile);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
