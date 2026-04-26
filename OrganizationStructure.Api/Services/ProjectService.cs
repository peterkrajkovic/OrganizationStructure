using Microsoft.EntityFrameworkCore;
using OrganizationStructure.Api.Data;
using OrganizationStructure.Api.Domain.Entities;
using OrganizationStructure.Api.DTOs.Projects;
using OrganizationStructure.Api.DTOs.Employees;
using OrganizationStructure.Api.Repositories;

namespace OrganizationStructure.Api.Services;

public class ProjectService : IProjectService
{
    private readonly IRepository<Project> _projectRepository;
    private readonly IRepository<Division> _divisionRepository;
    private readonly IRepository<Employee> _employeeRepository;
    private readonly ApplicationDbContext _context;

    public ProjectService(
        IRepository<Project> projectRepository,
        IRepository<Division> divisionRepository,
        IRepository<Employee> employeeRepository,
        ApplicationDbContext context)
    {
        _projectRepository = projectRepository;
        _divisionRepository = divisionRepository;
        _employeeRepository = employeeRepository;
        _context = context;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        var projects = await _projectRepository.GetAllAsync(p => p.Manager);
        return projects.Select(MapToDto);
    }

    public async Task<IEnumerable<ProjectDto>> GetByDivisionIdAsync(Guid divisionId)
    {
        var projects = await _context.Projects
            .Include(p => p.Manager)
            .Where(p => p.DivisionId == divisionId)
            .ToListAsync();
        
        return projects.Select(MapToDto);
    }

    public async Task<ProjectDto?> GetByIdAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id, p => p.Manager);
        return project is null ? null : MapToDto(project);
    }

    public async Task<ProjectDto> CreateAsync(CreateOrUpdateProjectDto dto)
    {
        if (!await _divisionRepository.ExistsAsync(dto.DivisionId))
        {
            throw new InvalidOperationException("Division not found");
        }

        if (!await _employeeRepository.ExistsAsync(dto.ManagerId))
        {
            throw new InvalidOperationException("Manager not found");
        }

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Code = dto.Code,
            DivisionId = dto.DivisionId,
            ManagerId = dto.ManagerId,
            CreatedAt = DateTime.UtcNow
        };

        await _projectRepository.AddAsync(project);
        
        var created = await _projectRepository.GetByIdAsync(project.Id, p => p.Manager);
        return MapToDto(created!);
    }

    public async Task<ProjectDto?> UpdateAsync(Guid id, CreateOrUpdateProjectDto dto)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project is null) return null;

        if (!await _divisionRepository.ExistsAsync(dto.DivisionId))
        {
            throw new InvalidOperationException("Division not found");
        }

        if (!await _employeeRepository.ExistsAsync(dto.ManagerId))
        {
            throw new InvalidOperationException("Manager not found");
        }

        project.Name = dto.Name;
        project.Code = dto.Code;
        project.DivisionId = dto.DivisionId;
        project.ManagerId = dto.ManagerId;
        project.UpdatedAt = DateTime.UtcNow;

        await _projectRepository.UpdateAsync(project);
        
        var updated = await _projectRepository.GetByIdAsync(id, p => p.Manager);
        return MapToDto(updated!);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project is null) return false;

        await _projectRepository.DeleteAsync(project);
        return true;
    }

    private static ProjectDto MapToDto(Project project) => new(
        project.Id,
        project.Name,
        project.Code,
        project.DivisionId,
        project.ManagerId,
        new EmployeeDto(
            project.Manager.Id,
            project.Manager.Title,
            project.Manager.FirstName,
            project.Manager.LastName,
            project.Manager.Phone,
            project.Manager.Email,
            project.Manager.CreatedAt,
            project.Manager.UpdatedAt
        ),
        project.CreatedAt,
        project.UpdatedAt
    );
}