// <copyright file="SqlNullableDateOnlyTypeHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LayerArchitectureExample.DataAccess.Core.Mapping;

using System;
using System.Data;

using Dapper;

public class SqlNullableDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly?>
{
    public static void Setup()
    {
        SqlMapper.RemoveTypeMap(typeof(DateOnly?));
        SqlMapper.AddTypeHandler(new SqlNullableDateOnlyTypeHandler());
    }

    public override void SetValue(IDbDataParameter parameter, DateOnly? value)
    {
        if (value is null)
        {
            parameter.Value = null;
        }
        else
        {
            parameter.Value = value.Value.ToDateTime(new TimeOnly(0, 0));
        }
    }

    public override DateOnly? Parse(object value)
    {
        if (value is null)
        {
            return null;
        }

        return DateOnly.FromDateTime((DateTime)value);
    }
}
