namespace LayerArchitectureExample.DataAccess.Core;

using System;
using System.Data;

using LayerArchitectureExample.DataAccess.Contracts.Core;

using Microsoft.Extensions.Configuration;

using MySqlConnector;

public class MySqlConnectionFactory : IDbConnectionFactory
{
    private readonly string connectionString;

    public MySqlConnectionFactory(IConfiguration configuration)
    {
        this.connectionString = configuration.GetConnectionString("MySQL");

        ArgumentNullException.ThrowIfNull(this.connectionString);
    }

    public MySqlConnectionFactory(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public IDbConnection CreateDbConnection()
    {
        return new MySqlConnection(this.connectionString);
    }
}
