namespace OrganizationStructure.Api.DTOs.Departments;

using OrganizationStructure.Api.DTOs.Employees;

public record DepartmentDto(
    Guid Id,
    string Name,
    string Code,
    Guid ProjectId,
    Guid ManagerId,
    EmployeeDto Manager,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);