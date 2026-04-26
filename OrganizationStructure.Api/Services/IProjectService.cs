using OrganizationStructure.Api.DTOs.Projects;

namespace OrganizationStructure.Api.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync();
    Task<IEnumerable<ProjectDto>> GetByDivisionIdAsync(Guid divisionId);
    Task<ProjectDto?> GetByIdAsync(Guid id);
    Task<ProjectDto> CreateAsync(CreateOrUpdateProjectDto dto);
    Task<ProjectDto?> UpdateAsync(Guid id, CreateOrUpdateProjectDto dto);
    Task<bool> DeleteAsync(Guid id);
}