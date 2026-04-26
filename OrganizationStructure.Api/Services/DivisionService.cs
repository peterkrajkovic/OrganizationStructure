using Microsoft.EntityFrameworkCore;
using OrganizationStructure.Api.Data;
using OrganizationStructure.Api.Domain.Entities;
using OrganizationStructure.Api.DTOs.Divisions;
using OrganizationStructure.Api.DTOs.Employees;
using OrganizationStructure.Api.Repositories;

namespace OrganizationStructure.Api.Services;

public class DivisionService : IDivisionService
{
    private readonly IRepository<Division> _divisionRepository;
    private readonly IRepository<Company> _companyRepository;
    private readonly IRepository<Employee> _employeeRepository;
    private readonly ApplicationDbContext _context;

    public DivisionService(
        IRepository<Division> divisionRepository,
        IRepository<Company> companyRepository,
        IRepository<Employee> employeeRepository,
        ApplicationDbContext context)
    {
        _divisionRepository = divisionRepository;
        _companyRepository = companyRepository;
        _employeeRepository = employeeRepository;
        _context = context;
    }

    public async Task<IEnumerable<DivisionDto>> GetAllAsync()
    {
        var divisions = await _divisionRepository.GetAllAsync(d => d.Manager);
        return divisions.Select(MapToDto);
    }

    public async Task<IEnumerable<DivisionDto>> GetByCompanyIdAsync(Guid companyId)
    {
        var divisions = await _context.Divisions
            .Include(d => d.Manager)
            .Where(d => d.CompanyId == companyId)
            .ToListAsync();
        
        return divisions.Select(MapToDto);
    }

    public async Task<DivisionDto?> GetByIdAsync(Guid id)
    {
        var division = await _divisionRepository.GetByIdAsync(id, d => d.Manager);
        return division is null ? null : MapToDto(division);
    }

    public async Task<DivisionDto> CreateAsync(CreateOrUpdateDivisionDto dto)
    {
        if (!await _companyRepository.ExistsAsync(dto.CompanyId))
        {
            throw new InvalidOperationException("Company not found");
        }

        if (!await _employeeRepository.ExistsAsync(dto.ManagerId))
        {
            throw new InvalidOperationException("Manager not found");
        }

        var division = new Division
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Code = dto.Code,
            CompanyId = dto.CompanyId,
            ManagerId = dto.ManagerId,
            CreatedAt = DateTime.UtcNow
        };

        await _divisionRepository.AddAsync(division);
        
        var created = await _divisionRepository.GetByIdAsync(division.Id, d => d.Manager);
        return MapToDto(created!);
    }

    public async Task<DivisionDto?> UpdateAsync(Guid id, CreateOrUpdateDivisionDto dto)
    {
        var division = await _divisionRepository.GetByIdAsync(id);
        if (division is null) return null;

        if (!await _companyRepository.ExistsAsync(dto.CompanyId))
        {
            throw new InvalidOperationException("Company not found");
        }

        if (!await _employeeRepository.ExistsAsync(dto.ManagerId))
        {
            throw new InvalidOperationException("Manager not found");
        }

        division.Name = dto.Name;
        division.Code = dto.Code;
        division.CompanyId = dto.CompanyId;
        division.ManagerId = dto.ManagerId;
        division.UpdatedAt = DateTime.UtcNow;

        await _divisionRepository.UpdateAsync(division);
        
        var updated = await _divisionRepository.GetByIdAsync(id, d => d.Manager);
        return MapToDto(updated!);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var division = await _divisionRepository.GetByIdAsync(id);
        if (division is null) return false;

        await _divisionRepository.DeleteAsync(division);
        return true;
    }

    private static DivisionDto MapToDto(Division division) => new(
        division.Id,
        division.Name,
        division.Code,
        division.CompanyId,
        division.ManagerId,
        new EmployeeDto(
            division.Manager.Id,
            division.Manager.Title,
            division.Manager.FirstName,
            division.Manager.LastName,
            division.Manager.Phone,
            division.Manager.Email,
            division.Manager.CreatedAt,
            division.Manager.UpdatedAt
        ),
        division.CreatedAt,
        division.UpdatedAt
    );
}