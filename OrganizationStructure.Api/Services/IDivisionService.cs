using OrganizationStructure.Api.DTOs.Divisions;

namespace OrganizationStructure.Api.Services;

public interface IDivisionService
{
    Task<IEnumerable<DivisionDto>> GetAllAsync();
    Task<IEnumerable<DivisionDto>> GetByCompanyIdAsync(Guid companyId);
    Task<DivisionDto?> GetByIdAsync(Guid id);
    Task<DivisionDto> CreateAsync(CreateOrUpdateDivisionDto dto);
    Task<DivisionDto?> UpdateAsync(Guid id, CreateOrUpdateDivisionDto dto);
    Task<bool> DeleteAsync(Guid id);
}