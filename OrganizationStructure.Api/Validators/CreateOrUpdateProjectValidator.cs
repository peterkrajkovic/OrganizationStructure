using FluentValidation;
using OrganizationStructure.Api.DTOs.Projects;

namespace OrganizationStructure.Api.Validators;

public class CreateOrUpdateProjectValidator : AbstractValidator<CreateOrUpdateProjectDto>
{
    public CreateOrUpdateProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required")
            .MaximumLength(200).WithMessage("Project name must not exceed 200 characters");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Project code is required")
            .Matches(@"^[A-Z0-9\-_]+$").WithMessage("Code must contain only uppercase letters, numbers, hyphens, and underscores")
            .MaximumLength(20).WithMessage("Project code must not exceed 20 characters");

        RuleFor(x => x.DivisionId)
            .NotEmpty().WithMessage("Division is required");

        RuleFor(x => x.ManagerId)
            .NotEmpty().WithMessage("Manager is required");
    }
}