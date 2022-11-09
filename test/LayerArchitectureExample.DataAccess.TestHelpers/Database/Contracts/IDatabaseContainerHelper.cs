namespace LayerArchitectureExample.DataAccess.TestHelpers.Database.Contracts;

using System;
using System.Threading.Tasks;

using DotNet.Testcontainers.Containers;

/// <summary>
/// Helper class for the database docker container
/// </summary>
public interface IDatabaseContainerHelper : IAsyncDisposable
{
    /// <summary>
    /// Create the database docker container
    /// </summary>
    /// <returns>The <see cref="TestcontainersContainer"/> object</returns>
    public TestcontainersContainer CreateContainer();

    /// <summary>
    /// Starts the database docker container and executes the CRATE SQL scripts
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task StartContainerAndSetupDatabaseAsync();
}
