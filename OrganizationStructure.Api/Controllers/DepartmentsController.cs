using Microsoft.AspNetCore.Mvc;
using OrganizationStructure.Api.DTOs.Departments;
using OrganizationStructure.Api.Services;

namespace OrganizationStructure.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    private readonly ILogger<DepartmentsController> _logger;

    public DepartmentsController(IDepartmentService departmentService, ILogger<DepartmentsController> logger)
    {
        _departmentService = departmentService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all departments
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DepartmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll([FromQuery] Guid? projectId = null)
    {
        var departments = projectId.HasValue
            ? await _departmentService.GetByProjectIdAsync(projectId.Value)
            : await _departmentService.GetAllAsync();
        
        return Ok(departments);
    }

    /// <summary>
    /// Gets department by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartmentDto>> GetById(Guid id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        if (department is null)
        {
            return NotFound(new { message = "Department not found" });
        }

        return Ok(department);
    }

    /// <summary>
    /// Creates a new department
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DepartmentDto>> Create([FromBody] CreateOrUpdateDepartmentDto dto)
    {
        try
        {
            var department = await _departmentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating department");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Updates an existing department
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DepartmentDto>> Update(Guid id, [FromBody] CreateOrUpdateDepartmentDto dto)
    {
        try
        {
            var department = await _departmentService.UpdateAsync(id, dto);
            if (department is null)
            {
                return NotFound(new { message = "Department not found" });
            }

            return Ok(department);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating department {DepartmentId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a department
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var deleted = await _departmentService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "Department not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting department {DepartmentId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }
}