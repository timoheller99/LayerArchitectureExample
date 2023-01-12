namespace LayerArchitectureExample.DataAccess.Contracts.Core;

using System;
using System.Data;

public interface IDbModel<TIdType>
{
    public TIdType Id { get; set; }

    public string GetEntityName()
    {
        var attribute = (EntityNameAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(EntityNameAttribute));

        if (attribute == null)
        {
            throw new ConstraintException($"Type {this.GetType()} does not have an {nameof(EntityNameAttribute)} attribute declared.");
        }

        return attribute.EntityName;
    }
}
