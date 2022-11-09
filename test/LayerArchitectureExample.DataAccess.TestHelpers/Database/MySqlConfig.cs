namespace LayerArchitectureExample.DataAccess.TestHelpers.Database;

using LayerArchitectureExample.DataAccess.TestHelpers.Database.Contracts;

public class MySqlConfig : IDatabaseConfig
{
    public int ExposedPort { get; set; }

    public string AdminPassword { get; set; }

    public string DatabaseName { get; set; }

    public string SqlScriptsPath { get; set; }
}
