namespace LayerArchitectureExample.DataAccess.IntegrationTests.Core;

using System;
using System.Threading.Tasks;

using LayerArchitectureExample.DataAccess.Contracts.Core;
using LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Model;
using LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Model.Contracts;
using LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Persistence;
using LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Persistence.Contracts;
using LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Service;
using LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Service.Contracts;
using LayerArchitectureExample.DataAccess.TestHelpers.Database.Contracts;

public abstract class BaseTestFixture<TRepository, TDbModel> : IAsyncDisposable
    where TDbModel : IDbModel<Guid>
    where TRepository : IGenericRepository<Guid, TDbModel>
{
    private readonly IDatabaseHelper databaseHelper;

    protected BaseTestFixture(IDatabaseHelper databaseHelper)
    {
        this.databaseHelper = databaseHelper;
    }

    public IDataAccessModelHelper DataAccessModelHelper { get; private set; }

    public IDataAccessServiceHelper DataAccessServiceHelper { get; private set; }

    public IDataAccessPersistenceHelper DataAccessPersistenceHelper { get; private set; }

    public abstract TRepository GetRepository();

    public abstract TDbModel GetValidDbModel();

    public abstract TDbModel GetInvalidDbModel();

    public abstract Task<TDbModel> PersistDbModel();

    public abstract void PerformValidUpdate(ref TDbModel entity);

    public abstract void PerformInvalidUpdate(ref TDbModel entity);

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        if (this.databaseHelper != null)
        {
            await this.databaseHelper.DisposeAsync();
        }
    }

    protected async Task Setup()
    {
        this.databaseHelper.CreateDatabaseContainer();
        await this.databaseHelper.StartContainerAndSetupDatabaseAsync();

        this.DataAccessModelHelper = new DataAccessModelHelper();
        this.DataAccessServiceHelper = new DataAccessServiceHelper(this.databaseHelper.GetDbConnectionString());

        this.DataAccessPersistenceHelper = new DataAccessPersistenceHelper(this.DataAccessModelHelper, this.DataAccessServiceHelper);
    }
}
