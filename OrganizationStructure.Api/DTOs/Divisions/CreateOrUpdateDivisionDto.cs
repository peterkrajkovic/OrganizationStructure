namespace OrganizationStructure.Api.DTOs.Divisions;

public record CreateOrUpdateDivisionDto(
    string Name,
    string Code,
    Guid CompanyId,
    Guid ManagerId
);