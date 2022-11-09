namespace LayerArchitectureExample.DataAccess.Core.Mapping;

using System;
using System.Data;

using Dapper;

public sealed class SqlGuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public static void Setup()
    {
        SqlMapper.RemoveTypeMap(typeof(Guid));
        SqlMapper.RemoveTypeMap(typeof(Guid?));
        SqlMapper.AddTypeHandler(new SqlGuidTypeHandler());
    }

    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
        ArgumentNullException.ThrowIfNull(parameter);

        parameter.Value = value.ToString();
    }

    public override Guid Parse(object value)
    {
        return new Guid((string)value);
    }
}
