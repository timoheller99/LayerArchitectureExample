namespace LayerArchitectureExample.DataAccess.Core;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using LayerArchitectureExample.DataAccess.Contracts.Core;

// TODO: Try out Dapper.SqlBuilder
public class MySqlQueryGenerator<TIdType, TDbModel> : ISqlQueryGenerator
{
    private const string IdConditionText = $"{nameof(IDbModel<TIdType>.Id)}=@{nameof(IDbModel<TIdType>.Id)}";

    protected MySqlQueryGenerator(string entityName)
    {
        this.EntityName = entityName;
    }

    protected string EntityName { get; }

    public string GenerateInsertQuery()
    {
        var propertyNames = GetModelPropertyNames().ToList();

        var propertiesText = string.Join(", ", propertyNames);
        var propertiesValuesText = string.Join(", ", propertyNames.Select(name => $"@{name}"));

        var queryText = this.InsertQuery(propertiesText, propertiesValuesText);
        return queryText;
    }

    public string GenerateSelectAllQuery()
    {
        var queryText = this.SelectAllQuery();
        return queryText;
    }

    public string GenerateSelectByIdQuery()
    {
        var queryText = this.SelectByConditionQuery(IdConditionText);
        return queryText;
    }

    public string GenerateUpdateQuery()
    {
        var propertyNames = GetModelPropertyNames();
        var setValuesText = string.Join(", ", propertyNames.Select(name => $"{name}=@{name}"));

        var queryText = this.UpdateQuery(setValuesText, IdConditionText);
        return queryText;
    }

    public string GenerateDeleteQuery()
    {
        var queryText = this.DeleteQuery(IdConditionText);
        return queryText;
    }

    public string GenerateSelectByPropertyQuery(string propertyName)
    {
        var conditionText = $"{propertyName}=@{propertyName}";

        var queryText = this.SelectByConditionQuery(conditionText);
        return queryText;
    }

    protected string GenerateSelectByTimespanQuery(string fromPropertyName, string untilPropertyName)
    {
        var conditionText =
            $"({fromPropertyName}>=@{fromPropertyName} AND {untilPropertyName}<=@{untilPropertyName}) OR {untilPropertyName} IS NULL";

        var queryText = this.SelectByConditionQuery(conditionText);
        return queryText;
    }

    protected string SelectByConditionQuery(string conditionText)
    {
        return $"SELECT * FROM {this.EntityName} WHERE {conditionText};";
    }

    protected string InsertQuery(string propertiesText, string valuesText)
    {
        return $"INSERT INTO {this.EntityName}({propertiesText}) VALUES({valuesText});";
    }

    protected string SelectAllQuery()
    {
        return $"SELECT * FROM {this.EntityName};";
    }

    protected string UpdateQuery(string setValuesText, string conditionText)
    {
        return $"UPDATE {this.EntityName} SET {setValuesText} WHERE {conditionText};";
    }

    protected string DeleteQuery(string conditionText)
    {
        return $"DELETE FROM {this.EntityName} WHERE {conditionText};";
    }

    private static IEnumerable<string> GetModelPropertyNames()
    {
        var properties = typeof(TDbModel).GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(info => info.Name);
        return properties;
    }
}
