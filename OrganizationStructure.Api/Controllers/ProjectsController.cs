using Microsoft.AspNetCore.Mvc;
using OrganizationStructure.Api.DTOs.Projects;
using OrganizationStructure.Api.Services;

namespace OrganizationStructure.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger)
    {
        _projectService = projectService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all projects
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProjectDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll([FromQuery] Guid? divisionId = null)
    {
        var projects = divisionId.HasValue
            ? await _projectService.GetByDivisionIdAsync(divisionId.Value)
            : await _projectService.GetAllAsync();
        
        return Ok(projects);
    }

    /// <summary>
    /// Gets project by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectDto>> GetById(Guid id)
    {
        var project = await _projectService.GetByIdAsync(id);
        if (project is null)
        {
            return NotFound(new { message = "Project not found" });
        }

        return Ok(project);
    }

    /// <summary>
    /// Creates a new project
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateOrUpdateProjectDto dto)
    {
        try
        {
            var project = await _projectService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Updates an existing project
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectDto>> Update(Guid id, [FromBody] CreateOrUpdateProjectDto dto)
    {
        try
        {
            var project = await _projectService.UpdateAsync(id, dto);
            if (project is null)
            {
                return NotFound(new { message = "Project not found" });
            }

            return Ok(project);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating project {ProjectId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a project
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var deleted = await _projectService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "Project not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting project {ProjectId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }
}