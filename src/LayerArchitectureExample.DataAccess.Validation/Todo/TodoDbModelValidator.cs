namespace LayerArchitectureExample.DataAccess.Validation.Todo;

using FluentValidation;

using LayerArchitectureExample.DataAccess.Contracts.Todo;
using LayerArchitectureExample.DataAccess.Validation.Core;

public class TodoDbModelValidator : AbstractValidator<TodoDbModel>
{
    public TodoDbModelValidator()
    {
        this.RuleFor(m => m.Id)
            .SetValidator(new GuidValidator());

        this.RuleFor(m => m.Name)
            .NotNull()
            .NotEmpty();

        this.RuleFor(m => m.TodoListId)
            .SetValidator(new GuidValidator());
    }
}
