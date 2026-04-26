using FluentValidation;
using OrganizationStructure.Api.DTOs.Companies;

namespace OrganizationStructure.Api.Validators;

public class CreateOrUpdateCompanyValidator : AbstractValidator<CreateOrUpdateCompanyDto>
{
    public CreateOrUpdateCompanyValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Company name is required")
            .MaximumLength(200).WithMessage("Company name must not exceed 200 characters");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Company code is required")
            .Matches(@"^[A-Z0-9\-_]+$").WithMessage("Code must contain only uppercase letters, numbers, hyphens, and underscores")
            .MaximumLength(20).WithMessage("Company code must not exceed 20 characters");

        RuleFor(x => x.DirectorId)
            .NotEmpty().WithMessage("Director is required");
    }
}