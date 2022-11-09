namespace LayerArchitectureExample.DataAccess.Core.Mapping;

using System;
using System.Data;

using Dapper;

public class SqlDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public static void Setup()
    {
        SqlMapper.RemoveTypeMap(typeof(DateOnly));
        SqlMapper.AddTypeHandler(new SqlDateOnlyTypeHandler());
    }

    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.Value = value.ToDateTime(new TimeOnly(0, 0));
    }

    public override DateOnly Parse(object value)
    {
        return DateOnly.FromDateTime((DateTime)value);
    }
}
