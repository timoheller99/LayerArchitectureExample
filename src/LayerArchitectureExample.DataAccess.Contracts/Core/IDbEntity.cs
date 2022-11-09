namespace LayerArchitectureExample.DataAccess.Contracts.Core;

public interface IDbEntity<TIdType>
{
    public TIdType Id { get; set; }
}
