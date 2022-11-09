namespace LayerArchitectureExample.DataAccess.Contracts.Todo;

using System;

using LayerArchitectureExample.DataAccess.Contracts.Core;

public class TodoDbModel : IDbModel<Guid>
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateOnly DueDate { get; set; }

    public Guid TodoListId { get; set; }
}
