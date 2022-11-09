namespace LayerArchitectureExample.DataAccess.Contracts.Core;

public interface ISqlQueryGenerator
{
    public string GenerateInsertQuery();

    public string GenerateSelectAllQuery();

    public string GenerateSelectByIdQuery();

    public string GenerateUpdateQuery();

    public string GenerateDeleteQuery();
}
