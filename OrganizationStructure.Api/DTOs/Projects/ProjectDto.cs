namespace OrganizationStructure.Api.DTOs.Projects;

using OrganizationStructure.Api.DTOs.Employees;

public record ProjectDto(
    Guid Id,
    string Name,
    string Code,
    Guid DivisionId,
    Guid ManagerId,
    EmployeeDto Manager,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);