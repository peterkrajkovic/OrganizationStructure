using Microsoft.AspNetCore.Mvc;
using OrganizationStructure.Api.DTOs.Companies;
using OrganizationStructure.Api.Services;

namespace OrganizationStructure.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;
    private readonly ILogger<CompaniesController> _logger;

    public CompaniesController(ICompanyService companyService, ILogger<CompaniesController> logger)
    {
        _companyService = companyService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAll()
    {
        var companies = await _companyService.GetAllAsync();
        return Ok(companies);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyDto>> GetById(Guid id)
    {
        var company = await _companyService.GetByIdAsync(id);
        if (company is null)
        {
            return NotFound(new { message = "Company not found" });
        }

        return Ok(company);
    }

    [HttpPost]
    public async Task<ActionResult<CompanyDto>> Create([FromBody] CreateOrUpdateCompanyDto dto)
    {
        try
        {
            var company = await _companyService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = company.Id }, company);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating company");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CompanyDto>> Update(Guid id, [FromBody] CreateOrUpdateCompanyDto dto)
    {
        try
        {
            var company = await _companyService.UpdateAsync(id, dto);
            if (company is null)
            {
                return NotFound(new { message = "Company not found" });
            }

            return Ok(company);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating company {CompanyId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var deleted = await _companyService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "Company not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting company {CompanyId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }
}