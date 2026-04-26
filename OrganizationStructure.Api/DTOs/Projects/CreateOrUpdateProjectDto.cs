namespace OrganizationStructure.Api.DTOs.Projects;

public record CreateOrUpdateProjectDto(
    string Name,
    string Code,
    Guid DivisionId,
    Guid ManagerId
);