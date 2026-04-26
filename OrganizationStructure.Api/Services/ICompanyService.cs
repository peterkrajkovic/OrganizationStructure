using OrganizationStructure.Api.DTOs.Companies;

namespace OrganizationStructure.Api.Services;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllAsync();
    Task<CompanyDto?> GetByIdAsync(Guid id);
    Task<CompanyDto> CreateAsync(CreateOrUpdateCompanyDto dto);
    Task<CompanyDto?> UpdateAsync(Guid id, CreateOrUpdateCompanyDto dto);
    Task<bool> DeleteAsync(Guid id);
}