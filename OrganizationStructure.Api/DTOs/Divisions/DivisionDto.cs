namespace OrganizationStructure.Api.DTOs.Divisions;

using OrganizationStructure.Api.DTOs.Employees;

public record DivisionDto(
    Guid Id,
    string Name,
    string Code,
    Guid CompanyId,
    Guid ManagerId,
    EmployeeDto Manager,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);