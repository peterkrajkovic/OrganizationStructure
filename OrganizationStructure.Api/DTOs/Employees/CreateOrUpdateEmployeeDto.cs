namespace OrganizationStructure.Api.DTOs.Employees;

public record CreateOrUpdateEmployeeDto(
    string Title,
    string FirstName,
    string LastName,
    string Phone,
    string Email
);