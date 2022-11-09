namespace LayerArchitectureExample.Core.Validation.Contracts;

using System.Threading.Tasks;

using FluentValidation.Results;

public interface IValidationService<TValidationModel>
{
    public Task<ValidationResult> ValidateAsync(TValidationModel model);
}
