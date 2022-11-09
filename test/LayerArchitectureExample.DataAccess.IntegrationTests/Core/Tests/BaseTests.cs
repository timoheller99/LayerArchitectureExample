namespace LayerArchitectureExample.DataAccess.IntegrationTests.Core.Tests;

using System;
using System.Diagnostics.CodeAnalysis;

using LayerArchitectureExample.DataAccess.Contracts.Core;

using Xunit;

[SuppressMessage("Naming", "MA0048", Justification = "Split up test methods for readability.")]
public abstract partial class BaseTests<TFixture, TRepository, TDbModel> : IClassFixture<TFixture>
    where TDbModel : class, IDbModel<Guid>, new()
    where TRepository : IGenericRepository<Guid, TDbModel>
    where TFixture : BaseTestFixture<TRepository, TDbModel>, new()
{
    protected BaseTests(TFixture fixture)
    {
        this.Fixture = fixture;
    }

    protected TFixture Fixture { get; }
}
