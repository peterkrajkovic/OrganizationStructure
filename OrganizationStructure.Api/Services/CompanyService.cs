using OrganizationStructure.Api.Domain.Entities;
using OrganizationStructure.Api.DTOs.Companies;
using OrganizationStructure.Api.DTOs.Employees;
using OrganizationStructure.Api.Repositories;

namespace OrganizationStructure.Api.Services;

public class CompanyService : ICompanyService
{
    private readonly IRepository<Company> _companyRepository;
    private readonly IRepository<Employee> _employeeRepository;

    public CompanyService(IRepository<Company> companyRepository, IRepository<Employee> employeeRepository)
    {
        _companyRepository = companyRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<CompanyDto>> GetAllAsync()
    {
        var companies = await _companyRepository.GetAllAsync(c => c.Director);
        return companies.Select(MapToDto);
    }

    public async Task<CompanyDto?> GetByIdAsync(Guid id)
    {
        var company = await _companyRepository.GetByIdAsync(id, c => c.Director);
        return company is null ? null : MapToDto(company);
    }

    public async Task<CompanyDto> CreateAsync(CreateOrUpdateCompanyDto dto)
    {
        if (!await _employeeRepository.ExistsAsync(dto.DirectorId))
        {
            throw new InvalidOperationException("Director not found");
        }

        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Code = dto.Code,
            DirectorId = dto.DirectorId,
            CreatedAt = DateTime.UtcNow
        };

        await _companyRepository.AddAsync(company);
        
        var created = await _companyRepository.GetByIdAsync(company.Id, c => c.Director);
        return MapToDto(created!);
    }

    public async Task<CompanyDto?> UpdateAsync(Guid id, CreateOrUpdateCompanyDto dto)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        if (company is null) return null;

        if (!await _employeeRepository.ExistsAsync(dto.DirectorId))
        {
            throw new InvalidOperationException("Director not found");
        }

        company.Name = dto.Name;
        company.Code = dto.Code;
        company.DirectorId = dto.DirectorId;
        company.UpdatedAt = DateTime.UtcNow;

        await _companyRepository.UpdateAsync(company);
        
        var updated = await _companyRepository.GetByIdAsync(id, c => c.Director);
        return MapToDto(updated!);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        if (company is null) return false;

        await _companyRepository.DeleteAsync(company);
        return true;
    }

    private static CompanyDto MapToDto(Company company) => new(
        company.Id,
        company.Name,
        company.Code,
        company.DirectorId,
        new EmployeeDto(
            company.Director.Id,
            company.Director.Title,
            company.Director.FirstName,
            company.Director.LastName,
            company.Director.Phone,
            company.Director.Email,
            company.Director.CreatedAt,
            company.Director.UpdatedAt
        ),
        company.CreatedAt,
        company.UpdatedAt
    );
}