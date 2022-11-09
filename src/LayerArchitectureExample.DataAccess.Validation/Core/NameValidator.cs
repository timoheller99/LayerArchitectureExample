namespace LayerArchitectureExample.DataAccess.Validation.Core;

using FluentValidation;

public class NameValidator : AbstractValidator<string>
{
    public NameValidator()
    {
        this.RuleFor(m => m)
            .Must(m => !string.IsNullOrWhiteSpace(m));
    }

    protected override void EnsureInstanceNotNull(object instanceToValidate)
    {
        // It's possible that instance that should be validated is null (default(string)).
        // Normally this would cause an ArgumentNullException thrown by FluentValidations Guard.
        // By overriding this method, it's possible to pass null as the instance to validate.
    }
}
