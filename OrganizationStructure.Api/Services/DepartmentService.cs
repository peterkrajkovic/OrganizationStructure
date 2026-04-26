using Microsoft.EntityFrameworkCore;
using OrganizationStructure.Api.Data;
using OrganizationStructure.Api.Domain.Entities;
using OrganizationStructure.Api.DTOs.Departments;
using OrganizationStructure.Api.DTOs.Employees;
using OrganizationStructure.Api.Repositories;

namespace OrganizationStructure.Api.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IRepository<Department> _departmentRepository;
    private readonly IRepository<Project> _projectRepository;
    private readonly IRepository<Employee> _employeeRepository;
    private readonly ApplicationDbContext _context;

    public DepartmentService(
        IRepository<Department> departmentRepository,
        IRepository<Project> projectRepository,
        IRepository<Employee> employeeRepository,
        ApplicationDbContext context)
    {
        _departmentRepository = departmentRepository;
        _projectRepository = projectRepository;
        _employeeRepository = employeeRepository;
        _context = context;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await _departmentRepository.GetAllAsync(d => d.Manager);
        return departments.Select(MapToDto);
    }

    public async Task<IEnumerable<DepartmentDto>> GetByProjectIdAsync(Guid projectId)
    {
        var departments = await _context.Departments
            .Include(d => d.Manager)
            .Where(d => d.ProjectId == projectId)
            .ToListAsync();
        
        return departments.Select(MapToDto);
    }

    public async Task<DepartmentDto?> GetByIdAsync(Guid id)
    {
        var department = await _departmentRepository.GetByIdAsync(id, d => d.Manager);
        return department is null ? null : MapToDto(department);
    }

    public async Task<DepartmentDto> CreateAsync(CreateOrUpdateDepartmentDto dto)
    {
        if (!await _projectRepository.ExistsAsync(dto.ProjectId))
        {
            throw new InvalidOperationException("Project not found");
        }

        if (!await _employeeRepository.ExistsAsync(dto.ManagerId))
        {
            throw new InvalidOperationException("Manager not found");
        }

        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Code = dto.Code,
            ProjectId = dto.ProjectId,
            ManagerId = dto.ManagerId,
            CreatedAt = DateTime.UtcNow
        };

        await _departmentRepository.AddAsync(department);
        
        var created = await _departmentRepository.GetByIdAsync(department.Id, d => d.Manager);
        return MapToDto(created!);
    }

    public async Task<DepartmentDto?> UpdateAsync(Guid id, CreateOrUpdateDepartmentDto dto)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department is null) return null;

        if (!await _projectRepository.ExistsAsync(dto.ProjectId))
        {
            throw new InvalidOperationException("Project not found");
        }

        if (!await _employeeRepository.ExistsAsync(dto.ManagerId))
        {
            throw new InvalidOperationException("Manager not found");
        }

        department.Name = dto.Name;
        department.Code = dto.Code;
        department.ProjectId = dto.ProjectId;
        department.ManagerId = dto.ManagerId;
        department.UpdatedAt = DateTime.UtcNow;

        await _departmentRepository.UpdateAsync(department);
        
        var updated = await _departmentRepository.GetByIdAsync(id, d => d.Manager);
        return MapToDto(updated!);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department is null) return false;

        await _departmentRepository.DeleteAsync(department);
        return true;
    }

    private static DepartmentDto MapToDto(Department department) => new(
        department.Id,
        department.Name,
        department.Code,
        department.ProjectId,
        department.ManagerId,
        new EmployeeDto(
            department.Manager.Id,
            department.Manager.Title,
            department.Manager.FirstName,
            department.Manager.LastName,
            department.Manager.Phone,
            department.Manager.Email,
            department.Manager.CreatedAt,
            department.Manager.UpdatedAt
        ),
        department.CreatedAt,
        department.UpdatedAt
    );
}