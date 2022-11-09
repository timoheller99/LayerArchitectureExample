namespace LayerArchitectureExample.DataAccess.TestHelpers.Database.Contracts;

/// <summary>
/// Config model for a Database
/// </summary>
public interface IDatabaseConfig
{
    /// <summary>
    /// Port on which the database server is running
    /// </summary>
    public int ExposedPort { get; set; }

    /// <summary>
    /// Password for the root user
    /// </summary>
    public string AdminPassword { get; set; }

    /// <summary>
    /// Name of the database
    /// </summary>
    public string DatabaseName { get; set; }

    /// <summary>
    /// Path for the CREATE scripts
    /// </summary>
    public string SqlScriptsPath { get; set; }
}
