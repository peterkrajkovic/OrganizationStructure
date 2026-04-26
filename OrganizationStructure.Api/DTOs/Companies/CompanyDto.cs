namespace OrganizationStructure.Api.DTOs.Companies;

using OrganizationStructure.Api.DTOs.Employees;

public record CompanyDto(
    Guid Id,
    string Name,
    string Code,
    Guid DirectorId,
    EmployeeDto Director,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);