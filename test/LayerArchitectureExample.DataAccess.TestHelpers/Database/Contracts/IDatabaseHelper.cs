namespace LayerArchitectureExample.DataAccess.TestHelpers.Database.Contracts;

using System;
using System.Threading.Tasks;

/// <summary>
/// Helper class for database concerns
/// </summary>
public interface IDatabaseHelper : IAsyncDisposable
{
    /// <summary>
    /// Creates the database docker container
    /// </summary>
    public void CreateDatabaseContainer();

    /// <summary>
    /// Starts the database docker container and executes the CRATE SQL scripts
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task StartContainerAndSetupDatabaseAsync();

    /// <summary>
    /// Get the connection string for the database
    /// </summary>
    /// <returns>The connection string</returns>
    public string GetDbConnectionString();
}
