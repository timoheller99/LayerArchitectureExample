namespace LayerArchitectureExample.DataAccess.Contracts.Core;

using System.Data;

public interface IDbConnectionFactory
{
    public IDbConnection CreateDbConnection();
}
