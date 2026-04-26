using OrganizationStructure.Api.DTOs.Departments;

namespace OrganizationStructure.Api.Services;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
    Task<IEnumerable<DepartmentDto>> GetByProjectIdAsync(Guid projectId);
    Task<DepartmentDto?> GetByIdAsync(Guid id);
    Task<DepartmentDto> CreateAsync(CreateOrUpdateDepartmentDto dto);
    Task<DepartmentDto?> UpdateAsync(Guid id, CreateOrUpdateDepartmentDto dto);
    Task<bool> DeleteAsync(Guid id);
}