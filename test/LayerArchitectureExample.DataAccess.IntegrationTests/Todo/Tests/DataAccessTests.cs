namespace LayerArchitectureExample.DataAccess.IntegrationTests.Todo.Tests;

using System;
using System.Diagnostics.CodeAnalysis;

using LayerArchitectureExample.DataAccess.Contracts.Core;
using LayerArchitectureExample.DataAccess.IntegrationTests.Core;
using LayerArchitectureExample.DataAccess.IntegrationTests.Core.Tests;

[SuppressMessage("Naming", "MA0048", Justification = "Split up test methods for readability.")]
public abstract partial class DataAccessTests<TFixture, TRepository, TDbModel> : BaseTests<TFixture, TRepository, TDbModel>
    where TDbModel : class, IDbModel<Guid>, new()
    where TRepository : IGenericRepository<Guid, TDbModel>
    where TFixture : BaseTestFixture<TRepository, TDbModel>, new()
{
    protected DataAccessTests(TFixture fixture)
        : base(fixture)
    {
    }
}
