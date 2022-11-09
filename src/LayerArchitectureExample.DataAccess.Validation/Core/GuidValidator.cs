namespace LayerArchitectureExample.DataAccess.Validation.Core;

using System;

using FluentValidation;

public class GuidValidator : AbstractValidator<Guid>
{
    public GuidValidator()
    {
        this.RuleFor(x => x)
            .NotEqual(Guid.Empty)
            .OverridePropertyName("Id");
    }
}
