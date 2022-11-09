namespace LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Model.Contracts;

using System;

using LayerArchitectureExample.DataAccess.Contracts.Todo;

public interface ITodoDataAccessModelHelper
{
    public TodoDbModel CreateValidTodoDbModel(Guid todoListId);

    public TodoDbModel CreateInvalidTodoDbModel();
}
