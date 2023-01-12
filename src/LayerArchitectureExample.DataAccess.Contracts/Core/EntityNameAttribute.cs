namespace LayerArchitectureExample.DataAccess.Contracts.Core;

using System;

[AttributeUsage(AttributeTargets.Class)]
public sealed class EntityNameAttribute : Attribute
{
    public EntityNameAttribute(string entityName)
    {
        this.EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
    }

    public string EntityName { get; }
}
