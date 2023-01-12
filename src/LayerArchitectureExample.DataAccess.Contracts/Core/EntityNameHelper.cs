namespace LayerArchitectureExample.DataAccess.Contracts.Core;

using System;
using System.Data;

public static class EntityNameHelper
{
    public static string GetEntityName<T>()
    {
        var attribute = (EntityNameAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(EntityNameAttribute));

        if (attribute == null)
        {
            throw new ConstraintException($"Type {typeof(T)} does not have an {nameof(EntityNameAttribute)} attribute declared.");
        }

        return attribute.EntityName;
    }
}
