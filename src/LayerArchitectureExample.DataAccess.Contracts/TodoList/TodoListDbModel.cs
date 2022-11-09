namespace LayerArchitectureExample.DataAccess.Contracts.TodoList;

using System;

using LayerArchitectureExample.DataAccess.Contracts.Core;

public class TodoListDbModel : IDbModel<Guid>
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateOnly DueDate { get; set; }
}
