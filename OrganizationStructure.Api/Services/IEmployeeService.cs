using OrganizationStructure.Api.DTOs.Employees;

namespace OrganizationStructure.Api.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto?> GetByIdAsync(Guid id);
    Task<EmployeeDto> CreateAsync(CreateOrUpdateEmployeeDto dto);
    Task<EmployeeDto?> UpdateAsync(Guid id, CreateOrUpdateEmployeeDto dto);
    Task<bool> DeleteAsync(Guid id);
}