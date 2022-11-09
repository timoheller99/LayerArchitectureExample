namespace LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Model;

using System;

using LayerArchitectureExample.DataAccess.Contracts.Todo;
using LayerArchitectureExample.DataAccess.Contracts.TodoList;
using LayerArchitectureExample.DataAccess.TestHelpers.DataAccess.Model.Contracts;

public class DataAccessModelHelper : IDataAccessModelHelper
{
    private static Guid ValidId => Guid.NewGuid();

    private static Guid InvalidId => Guid.Empty;

    private static string ValidName => Guid.NewGuid().ToString();

    private static string InvalidName => null;

    private static DateOnly ValidDueDate => DateOnly.FromDateTime(DateTime.Today.AddDays(1));

    public TodoListDbModel CreateValidTodoListDbModel()
    {
        return new TodoListDbModel
        {
            Id = ValidId,
            Name = ValidName,
            DueDate = ValidDueDate,
        };
    }

    public TodoListDbModel CreateInvalidTodoListDbModel()
    {
        return new TodoListDbModel
        {
            Id = InvalidId,
            Name = InvalidName,
            DueDate = ValidDueDate,
        };
    }

    public TodoDbModel CreateValidTodoDbModel(Guid todoListId)
    {
        return new TodoDbModel
        {
            Id = ValidId,
            Name = ValidName,
            DueDate = ValidDueDate,
            TodoListId = todoListId,
        };
    }

    public TodoDbModel CreateInvalidTodoDbModel()
    {
        return new TodoDbModel
        {
            Id = InvalidId,
            Name = InvalidName,
            DueDate = ValidDueDate,
            TodoListId = InvalidId,
        };
    }
}
