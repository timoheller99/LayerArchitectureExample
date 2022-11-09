namespace LayerArchitectureExample.DataAccess.TestHelpers.Database;

using System;
using System.IO;
using System.Threading.Tasks;

using LayerArchitectureExample.DataAccess.TestHelpers.Database.Contracts;

public class DatabaseHelper : IDatabaseHelper
{
    private const string DatabaseName = "LayerArchitectureExample";

    private readonly string sqlScriptsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "createScripts", "MySql");

    private readonly IDatabaseContainerHelper containerHelper;

    private readonly string dbConnectionString;

    public DatabaseHelper()
    {
        var config = new MySqlConfig
        {
            ExposedPort = PortHelper.GetNextPort(),
            AdminPassword = "root",
            DatabaseName = DatabaseName,
            SqlScriptsPath = this.sqlScriptsFolder,
        };

        this.dbConnectionString = $"Server=localhost;Port={config.ExposedPort};Database={config.DatabaseName};Uid=root;Pwd={config.AdminPassword};";

        this.containerHelper = new DatabaseContainerHelper(config);
    }

    public void CreateDatabaseContainer()
    {
        this.containerHelper.CreateContainer();
    }

    public async Task StartContainerAndSetupDatabaseAsync()
    {
        await this.containerHelper.StartContainerAndSetupDatabaseAsync();
    }

    public string GetDbConnectionString()
    {
        return this.dbConnectionString;
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        if (this.containerHelper != null)
        {
            await this.containerHelper.DisposeAsync();
        }
    }
}
