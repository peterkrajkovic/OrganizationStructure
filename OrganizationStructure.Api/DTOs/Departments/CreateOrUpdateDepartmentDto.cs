namespace OrganizationStructure.Api.DTOs.Departments;

public record CreateOrUpdateDepartmentDto(
    string Name,
    string Code,
    Guid ProjectId,
    Guid ManagerId
);