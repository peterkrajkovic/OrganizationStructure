namespace OrganizationStructure.Api.DTOs.Companies;

public record CreateOrUpdateCompanyDto(
    string Name,
    string Code,
    Guid DirectorId
);