using Microsoft.AspNetCore.Mvc;
using OrganizationStructure.Api.DTOs.Divisions;
using OrganizationStructure.Api.Services;

namespace OrganizationStructure.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DivisionsController : ControllerBase
{
    private readonly IDivisionService _divisionService;
    private readonly ILogger<DivisionsController> _logger;

    public DivisionsController(IDivisionService divisionService, ILogger<DivisionsController> logger)
    {
        _divisionService = divisionService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all divisions
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DivisionDto>>> GetAll([FromQuery] Guid? companyId = null)
    {
        var divisions = companyId.HasValue
            ? await _divisionService.GetByCompanyIdAsync(companyId.Value)
            : await _divisionService.GetAllAsync();
        
        return Ok(divisions);
    }

    /// <summary>
    /// Gets division by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<DivisionDto>> GetById(Guid id)
    {
        var division = await _divisionService.GetByIdAsync(id);
        if (division is null)
        {
            return NotFound(new { message = "Division not found" });
        }

        return Ok(division);
    }

    /// <summary>
    /// Creates a new division
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<DivisionDto>> Create([FromBody] CreateOrUpdateDivisionDto dto)
    {
        try
        {
            var division = await _divisionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = division.Id }, division);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating division");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Updates an existing division
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<DivisionDto>> Update(Guid id, [FromBody] CreateOrUpdateDivisionDto dto)
    {
        try
        {
            var division = await _divisionService.UpdateAsync(id, dto);
            if (division is null)
            {
                return NotFound(new { message = "Division not found" });
            }

            return Ok(division);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating division {DivisionId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a division
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var deleted = await _divisionService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "Division not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting division {DivisionId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }
}   