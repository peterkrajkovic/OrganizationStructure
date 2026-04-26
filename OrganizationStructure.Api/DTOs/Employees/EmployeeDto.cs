namespace OrganizationStructure.Api.DTOs.Employees;

public record EmployeeDto(
    Guid Id,
    string Title,
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
