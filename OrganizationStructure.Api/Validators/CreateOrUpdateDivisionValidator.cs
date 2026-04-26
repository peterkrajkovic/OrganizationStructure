using FluentValidation;
using OrganizationStructure.Api.DTOs.Divisions;

namespace OrganizationStructure.Api.Validators;

public class CreateOrUpdateDivisionValidator : AbstractValidator<CreateOrUpdateDivisionDto>
{
    public CreateOrUpdateDivisionValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Division name is required")
            .MaximumLength(200).WithMessage("Division name must not exceed 200 characters");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Division code is required")
            .Matches(@"^[A-Z0-9\-_]+$").WithMessage("Code must contain only uppercase letters, numbers, hyphens, and underscores")
            .MaximumLength(20).WithMessage("Division code must not exceed 20 characters");

        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("Company is required");

        RuleFor(x => x.ManagerId)
            .NotEmpty().WithMessage("Manager is required");
    }
}