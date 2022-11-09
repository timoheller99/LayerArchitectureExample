namespace LayerArchitectureExample.DataAccess.Contracts.Core;

public interface IDbModel<TIdType>
{
    public TIdType Id { get; set; }
}
