using OrganizationStructure.Api.Data;
using OrganizationStructure.Api.Domain.Entities;
using OrganizationStructure.Api.DTOs.Employees;
using OrganizationStructure.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace OrganizationStructure.Api.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IRepository<Employee> _repository;
    private readonly ApplicationDbContext _context;

    public EmployeeService(IRepository<Employee> repository, ApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _repository.GetAllAsync();
        return employees.Select(MapToDto);
    }

    public async Task<EmployeeDto?> GetByIdAsync(Guid id)
    {
        var employee = await _repository.GetByIdAsync(id);
        return employee is null ? null : MapToDto(employee);
    }

    public async Task<EmployeeDto> CreateAsync(CreateOrUpdateEmployeeDto dto)
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Phone = dto.Phone,
            Email = dto.Email,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(employee);
        return MapToDto(employee);
    }

    public async Task<EmployeeDto?> UpdateAsync(Guid id, CreateOrUpdateEmployeeDto dto)
    {
        var employee = await _repository.GetByIdAsync(id);
        if (employee is null) return null;

        employee.Title = dto.Title;
        employee.FirstName = dto.FirstName;
        employee.LastName = dto.LastName;
        employee.Phone = dto.Phone;
        employee.Email = dto.Email;
        employee.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(employee);
        return MapToDto(employee);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var isUsed = await _context.Companies.AnyAsync(c => c.DirectorId == id) ||
                     await _context.Divisions.AnyAsync(d => d.ManagerId == id) ||
                     await _context.Projects.AnyAsync(p => p.ManagerId == id) ||
                     await _context.Departments.AnyAsync(d => d.ManagerId == id);

        if (isUsed)
        {
            throw new InvalidOperationException("Cannot delete employee who is assigned as a manager or director");
        }

        var employee = await _repository.GetByIdAsync(id);
        if (employee is null) return false;

        await _repository.DeleteAsync(employee);
        return true;
    }

    private static EmployeeDto MapToDto(Employee employee) => new(
        employee.Id,
        employee.Title,
        employee.FirstName,
        employee.LastName,
        employee.Phone,
        employee.Email,
        employee.CreatedAt,
        employee.UpdatedAt
    );
}