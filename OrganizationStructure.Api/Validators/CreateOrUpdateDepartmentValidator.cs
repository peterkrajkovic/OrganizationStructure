using FluentValidation;
using OrganizationStructure.Api.DTOs.Departments;

namespace OrganizationStructure.Api.Validators;

public class CreateOrUpdateDepartmentValidator : AbstractValidator<CreateOrUpdateDepartmentDto>
{
    public CreateOrUpdateDepartmentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Department name is required")
            .MaximumLength(200).WithMessage("Department name must not exceed 200 characters");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Department code is required")
            .Matches(@"^[A-Z0-9\-_]+$").WithMessage("Code must contain only uppercase letters, numbers, hyphens, and underscores")
            .MaximumLength(20).WithMessage("Department code must not exceed 20 characters");

        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("Project is required");

        RuleFor(x => x.ManagerId)
            .NotEmpty().WithMessage("Manager is required");
    }
}