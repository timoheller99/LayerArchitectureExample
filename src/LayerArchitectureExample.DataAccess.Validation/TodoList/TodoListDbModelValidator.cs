namespace LayerArchitectureExample.DataAccess.Validation.TodoList;

using FluentValidation;

using LayerArchitectureExample.DataAccess.Contracts.TodoList;
using LayerArchitectureExample.DataAccess.Validation.Core;

public class TodoListDbModelValidator : AbstractValidator<TodoListDbModel>
{
    public TodoListDbModelValidator()
    {
        this.RuleFor(m => m.Id)
            .SetValidator(new GuidValidator());

        this.RuleFor(m => m.Name)
            .NotNull()
            .NotEmpty();
    }
}
