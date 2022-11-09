namespace LayerArchitectureExample.Core.Validation;

using System.Threading.Tasks;

using FluentValidation;
using FluentValidation.Results;

using LayerArchitectureExample.Core.Validation.Contracts;

public class ValidationService<TValidator, TValidationModel> : IValidationService<TValidationModel>
    where TValidator : AbstractValidator<TValidationModel>
{
    private readonly TValidator validator;

    public ValidationService(TValidator validator)
    {
        this.validator = validator;
    }

    public async Task<ValidationResult> ValidateAsync(TValidationModel model)
    {
        return await this.validator.ValidateAsync(model);
    }
}
